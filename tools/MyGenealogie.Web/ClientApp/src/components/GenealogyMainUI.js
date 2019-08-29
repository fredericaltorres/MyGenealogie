import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Link } from 'react-router-dom';
import isObject from 'lodash/isObject';

// https://react-bootstrap.github.io/components/dropdowns/
import Dropdown from 'react-bootstrap/Dropdown';
import ButtonGroup from 'react-bootstrap/ButtonGroup';
import Button from 'react-bootstrap/Button';
import Alert from 'react-bootstrap/Alert';

// https://emotion.sh/docs/introduction
// https://github.com/JedWatson/react-select
import Select from 'react-select';

import {
    isPersonDate,
    emptyStringOnNull,
    replaceDash,
    formatYear,
    stringDateToPersonDate,
    PersonDBClient,
    PASTE_OPERATION_AS,
} from "./PersonDBClient";

const __personDBClient = new PersonDBClient();
const DEFAULT_IMAGE_WIDTH = 150;
const DEFAULT_PERSON_TO_SELECT = "eb6db547-abee-42ec-89c3-da273f8e30f3";
const APP_STATUS_READY = "Ready...";
const APP_STATUS_BUSY = "Busy...";


class GenealogyMainUI extends Component {

    state = {
        persons: [],
        selectedPerson: null,
        clipBoardPerson: null,
        applicationStatus: "Loading...",
    };

    componentDidMount() {

        this.reloadData();
    }

    personHistory = []; // guid list of all persons viewed

    setAppStatus(status) {

        this.updateState("applicationStatus", status);
    }

    userError(errorMessage) {

        alert(`ERROR:${errorMessage}`);
    }

    handleChange = (guid) => {

        if (guid === null) {

            this.updateState("selectedPerson", null);
            return;
        }

        if (isObject(guid)) {

            guid = guid.value;
        }

        var selectedPerson = { ...this.getPersonFromGuid(guid) };
        if (selectedPerson) {

            console.log(`handleChange  ${JSON.stringify(selectedPerson)}`);

            this.personHistory.push(selectedPerson.guid);
            console.log(`personHistory  ${JSON.stringify(this.personHistory)}`);

            this.updateState("selectedPerson", selectedPerson);
        }
        else {
            this.userError(`Cannot select person guid:${guid}`);
        }
    };

    updateSelectedPerson = () => {

        const person = { ...this.state.selectedPerson };
        person.birthDate = stringDateToPersonDate(person.birthDate);
        person.deathDate = stringDateToPersonDate(person.deathDate);
        return __personDBClient.updatePerson(person); // Return promise
    }

    pasteSelectedPersonFromClipboardAs = (pasteAsEnum) => {

        if (this.state.clipBoardPerson !== null) {

            console.log(`Paste '${__personDBClient.getPersonFullName(this.state.clipBoardPerson)}' as ${pasteAsEnum} into '${__personDBClient.getPersonFullName(this.state.selectedPerson)}'`);

            switch (pasteAsEnum) {

                case PASTE_OPERATION_AS.Father: break;
                case PASTE_OPERATION_AS.Mother: break;
                case PASTE_OPERATION_AS.Child: break;
                case PASTE_OPERATION_AS.Spouse: break;
            }
        }
    }

    copySelectedPersonToClipboard = () => {

        this.state.clipBoardPerson = { ...this.state.selectedPerson };
        console.log(`Copied to clipboard person:${__personDBClient.getPersonFullName(this.state.clipBoardPerson)}`);
        this.updateState("clipBoardPerson", this.state.clipBoardPerson);
    }

    goBackToPreviousPerson = () => {

        if (this.personHistory.length > 1) {

            var currentPersonGuid = this.personHistory.pop();
            var previousPersonGuid = this.personHistory.pop();
            this.selectPerson(previousPersonGuid);
        }
    }

    selectPerson = (guid) => {

        this.handleChange(guid);
    }

    getPersonFromGuid(guid) {

        return this.state.persons.find((p) => p.guid === guid);
    }

    getPersonSelected() {

        if (this.state.selectedPerson)
            return this.state.selectedPerson;

        return null;
    }

    isPersonHasSpouse(p) {

        return p.spouseGuid !== null;
    }

    isPersonHasFather(p) {

        return p.fatherGuid !== null;
    }

    isPersonHasMother(p) {

        return p.motherGuid !== null;
    }

    getSpouseForPersonSelected(personSelected) {

        return this.state.persons.find((p) => p.spouseGuid === personSelected.guid);
    }

    getFatherForPersonSelected(personSelected) {

        return this.state.persons.find((p) => p.guid === personSelected.fatherGuid);
    }

    getMotherForPersonSelected(personSelected) {

        return this.state.persons.find((p) => p.guid === personSelected.motherGuid);
    }

    getChildrenForPersonSelected(personSelected) {

        return this.state.persons.filter((p) => p.fatherGuid === personSelected.guid || p.motherGuid === personSelected.guid);
    }

    selectDefaultPerson() {

        return new Promise((resolve, reject) => {
            if (DEFAULT_PERSON_TO_SELECT)
                this.selectPerson(DEFAULT_PERSON_TO_SELECT);
            resolve();
        });
    }

    reloadData = () => {
        // +++
        this.setAppStatus(APP_STATUS_BUSY);
        __personDBClient.loadPersons().then((persons) => {
            this.updateState('persons', persons, () => {
                this.selectDefaultPerson().then(() => {
                    this.setAppStatus(APP_STATUS_READY);
                });
            });
        });

        //return fetch('api/MyGenealogie/GetPersons').then(response => response.json())
        //    .then(data => {
        //        // console.log(`reloadData data:${JSON.stringify(data)}`);
        //        this.updateState('persons', data, () => {
        //            this.selectDefaultPerson();
        //            this.setAppStatus(APP_STATUS_READY);
        //        });
        //    });
    }

    onPersonUpdated = (person) => {

        console.log(`onPersonUpdated person:${__personDBClient.getPersonFullName(person)}`);
        var persons = this.state.persons;
        const index = persons.findIndex((p) => { return p.guid === person.guid; });
        if (index === -1) {
            this.userError(`Cannot find person in memory guid:${person.guid}, fullName:${__personDBClient.getPersonFullName(person)} `);

            return false;
        }
        else {
            persons[index] = person;
            this.updateState("persons", persons);

            return true;
        }
    };

    onPersonCreated = (person) => {

        console.log(`onPersonCreated person:${__personDBClient.getPersonFullName(person)}`);
        var persons = this.state.persons;
        persons.push(person);
        this.updateState("persons", persons);

        return true;
    };

    onPersonDeleted = (person) => {

        person = this.getPersonFromGuid(person.guid); // Get the extact instance from the state
        console.log(`onPersonDeleted person:${__personDBClient.getPersonFullName(person)}`);
        var persons = this.state.persons;
        const index = persons.indexOf(person);
        if (index === -1) {
            console.error(`Cannot delete person from state person:${__personDBClient.getPersonFullName(person)}`);
        }
        else {
            persons.splice(index, 1);
            this.updateState("persons", persons);
        }
        return true;
    };

    updateState = (property, value, callBack = () => { }) => {

        if (typeof (property) === 'string')
            this.setState({ ...this.state, [property]: value }, callBack);

        if (typeof (property) === 'object') {

            let newState = { ...this.state };
            Object.key(property).forEach((key) => {
                newState = { ...newState, [key]: property[key] };
            });
            console.log(`new state:${JSON.stringify(newState)}`);
            this.setState(newState, callBack);
        }
    }

    GetPersonsSelector() {

        var r = this.state.persons.map((p) => {
            return (<li key={p.guid}>
                {__personDBClient.getPersonFullName(p)}
            </li>);
        });

        return <ul>{r}</ul>;
    }

    GetPersonsDataForCombo() {

        var r = this.state.persons.map((p) => {
            return {
                value: p.guid,
                label: __personDBClient.getPersonFullName(p)
            };
        });

        return r;
    }

    getPersonImagesHtml(person) {

        if (!person.images)
            return [];
        return person.images.map((image) => {
            return <img key={image.url} src={image.url} width={DEFAULT_IMAGE_WIDTH} />;
        });
    }

    getPersonSpouseSummaryHtml(person) {

        if (this.isPersonHasSpouse(person)) {
            const spouse = this.getSpouseForPersonSelected(person);
            return (<span>
                <button type="button" className="btn btn-primary" onClick={() => { this.selectPerson(this.getSpouseForPersonSelected(person).guid); }}> View </button>
                &nbsp;{__personDBClient.getPersonFullName(spouse)}
            </span>);
        }

        return null;
    }

    getPersonFatherSummaryHtml(person) {

        if (this.isPersonHasFather(person)) {
            const father = this.getFatherForPersonSelected(person);
            return (<span>
                <button type="button" className="btn btn-primary" onClick={() => { this.selectPerson(this.getFatherForPersonSelected(person).guid); }}> View </button>
                &nbsp;{__personDBClient.getPersonFullName(father)}
            </span>);
        }

        return null;
    }

    getPersonMotherSummaryHtml(person) {

        if (this.isPersonHasMother(person)) {
            const mother = this.getMotherForPersonSelected(person);
            return (<span>
                <button type="button" className="btn btn-primary" onClick={() => { this.selectPerson(this.getMotherForPersonSelected(person).guid); }}> View </button>
                &nbsp;{__personDBClient.getPersonFullName(mother)}
            </span>);
        }

        return null;
    }

    hasPersonChildren(person) {

        return this.getChildrenForPersonSelected(person).length > 0;
    }

    getPersonChildrenSummaryHtml(person) {

        const childrenHtml = this.getChildrenForPersonSelected(person).map((c) => {
            return (<li key={c.guid}>
                <button type="button" className="btn btn-primary" onClick={() => { this.selectPerson(c.guid); }} > View </button>
                &nbsp;{__personDBClient.getPersonFullName(c)}
            </li>);
        });

        return <ul>{childrenHtml}</ul>;
    }

    getBlockRow(fieldName, jsx) {

        return (<div className="form-group row">
            <label htmlFor={fieldName} className="col-sm-2 col-form-label">{fieldName}</label>
            <div className="col-sm-10">
                {jsx}
            </div>
        </div>);
    }

    onFieldChange = (e, fieldName, isDate) => {

        const value = e.target.value;
        console.log(`${fieldName} = ${value}`);

        const selectedPerson = this.state.selectedPerson;
        selectedPerson[fieldName] = value;
        const newState = { ...this.state, selectedPerson };
        this.setState(newState);
    }

    getFieldRow(fieldName, fieldValue, isMultiLine = false, isDate) {

        if (isPersonDate(fieldValue))
            fieldValue = formatYear(fieldValue);

        fieldValue = emptyStringOnNull(fieldValue);

        if (isMultiLine) {
            return (<div className="form-group row">
                <label htmlFor={fieldName} className="col-sm-2 col-form-label">{fieldName}</label>
                <div className="col-sm-10">
                    <textarea cols="80" rows="4" className="form-control-sm" id={fieldName} value={fieldValue}
                        onChange={(e) => { this.onFieldChange(e, fieldName); }}
                    />
                </div>
            </div>);
        }
        else {

            return (<div className="form-group row">
                <label htmlFor={fieldName} className="col-sm-2 col-form-label">{fieldName} : </label>
                <div className="col-sm-10">
                    <input type="text" pattern="[0-9. -]*" className="form-control" id={fieldName} value={fieldValue}
                        onChange={(e) => { this.onFieldChange(e, fieldName, isDate); }}
                    />
                </div>
            </div>);
        }
    }

    getPersonHtml(person) {

        return (<form>
            {this.getFieldRow("lastName", person.lastName)}
            {this.getFieldRow("maidenName", person.maidenName)}
            {this.getFieldRow("firstName", person.firstName)}
            {this.getFieldRow("middleName", replaceDash(person.middleName))}
            {this.getFieldRow("birthDate", person.birthDate, false, true)}
            {this.getFieldRow("deathDate", person.deathDate, false, true)}
            {this.getFieldRow("comment", person.comment, true)}

            {this.isPersonHasSpouse(person) &&
                this.getBlockRow("Spouse", this.getPersonSpouseSummaryHtml(person))}
            {this.isPersonHasFather(person) &&
                this.getBlockRow("Father", this.getPersonFatherSummaryHtml(person))}
            {this.isPersonHasMother(person) &&
                this.getBlockRow("Mother", this.getPersonMotherSummaryHtml(person))}
            {this.hasPersonChildren(person) &&
                this.getBlockRow("Children", this.getPersonChildrenSummaryHtml(person))}

        </form>);
    }

    render() {

        let selectionForComboBox = null;
        const personSelected = this.getPersonSelected();

        if (personSelected) {

            selectionForComboBox = {
                value: personSelected.guid,
                label: __personDBClient.getPersonFullName(personSelected)
            };
        }

        return (
            <div>
                <h2>MyGenealogy</h2>

                <Alert variant='primary'>
                    {this.state.applicationStatus}
                </Alert>

                <table>
                    <tbody>
                        <tr>
                            <td><button type="button" className="btn btn-primary" onClick={() => { this.goBackToPreviousPerson(); }}> Back </button> &nbsp;</td>
                            <td><button type="button" className="btn btn-primary" onClick={() => { this.copySelectedPersonToClipboard(); }}> Copy To Clipboard </button></td>
                            <td>
                                <button type="button" className="btn btn-primary" onClick={() => {
                                    this.setAppStatus(APP_STATUS_BUSY);
                                    this.updateSelectedPerson().then((person) => {
                                        this.onPersonUpdated(person);
                                        this.setAppStatus(APP_STATUS_READY);
                                    });
                                }}> Update </button>
                            </td>
                            <td>
                                <Dropdown>
                                    <Dropdown.Toggle variant="success" id="dropdown-basic">
                                        Operations - {__personDBClient.getPersonFullName(this.state.clipBoardPerson)}
                                    </Dropdown.Toggle>

                                    <Dropdown.Menu>
                                        <Dropdown.Item onClick={() => { this.pasteSelectedPersonFromClipboardAs(PASTE_OPERATION_AS.Father); }} >Paste as Father </Dropdown.Item>
                                        <Dropdown.Item onClick={() => { this.pasteSelectedPersonFromClipboardAs(PASTE_OPERATION_AS.Mother); }} >Paste as Mother </Dropdown.Item>
                                        <Dropdown.Item onClick={() => { this.pasteSelectedPersonFromClipboardAs(PASTE_OPERATION_AS.Child); }} >Paste as child </Dropdown.Item>
                                        <Dropdown.Item onClick={() => { this.pasteSelectedPersonFromClipboardAs(PASTE_OPERATION_AS.Spouse); }} >Paste as Spouse </Dropdown.Item>
                                    </Dropdown.Menu>
                                </Dropdown>
                            </td>
                            <td>
                                <ButtonGroup aria-label="Basic example">
                                    <Button variant="secondary" onClick={() => {
                                        this.setAppStatus(APP_STATUS_BUSY);
                                        __personDBClient.newPerson().then((newPerson) => {

                                            this.onPersonCreated(newPerson);
                                            this.setAppStatus(APP_STATUS_READY);
                                        });
                                    }}> New </Button>
                                    <Button variant="secondary" onClick={() => {
                                        this.setAppStatus(APP_STATUS_BUSY);
                                        const person = this.getPersonSelected();
                                        if (window.confirm(`Delete ${__personDBClient.getPersonFullName(person)}`)) {

                                            __personDBClient.deletePerson(this.getPersonSelected()).then(() => {

                                                this.onPersonDeleted(person);
                                                this.selectDefaultPerson();
                                                this.setAppStatus(APP_STATUS_READY);
                                            });
                                        }
                                    }}> Delete </Button>
                                    <Button variant="secondary">Clone</Button>
                                </ButtonGroup>
                            </td>
                        </tr>
                    </tbody>
                </table>

                <Select
                    isClearable isSearchable
                    value={selectionForComboBox}
                    onChange={this.handleChange}
                    options={this.GetPersonsDataForCombo()}
                />
                <hr />
                {personSelected && this.getPersonHtml(personSelected)}
                {personSelected && this.getPersonImagesHtml(personSelected)}

                {this.GetPersonsSelector()}
            </div>
        );
    }
}

export default connect()(GenealogyMainUI);
