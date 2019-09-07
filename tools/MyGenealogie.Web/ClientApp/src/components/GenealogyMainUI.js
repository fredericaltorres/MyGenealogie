import React, { Component } from 'react';
import { connect } from 'react-redux';
// import { bindActionCreators } from 'redux';
// import { Link } from 'react-router-dom';
import isObject from 'lodash/isObject';

// https://react-bootstrap.github.io/components/dropdowns/
import Dropdown from 'react-bootstrap/Dropdown';
import ButtonGroup from 'react-bootstrap/ButtonGroup';
import Button from 'react-bootstrap/Button';
import Alert from 'react-bootstrap/Alert';

// https://emotion.sh/docs/introduction
// https://github.com/JedWatson/react-select
import Select from 'react-select';

// https://github.com/ayrton/react-key-handler
// https://www.npmjs.com/package/react-key-handler
import KeyHandler, { KEYDOWN, KEYUP } from 'react-key-handler'; // KEYPRESS, 

import {
    isPersonDate,
    emptyStringOnNull,
    replaceDash,
    personDateToString,
    stringDateToPersonDate,
    PersonDBClient,
    PASTE_OPERATION_AS,
} from "./PersonDBClient";

const __personDBClient = new PersonDBClient();
const DEFAULT_IMAGE_WIDTH = 150;
const DEFAULT_USER_NAME = "fredericaltorres";
//const DEFAULT_PERSON_TO_SELECT = "eb6db547-abee-42ec-89c3-da273f8e30f3"; // leon
const DEFAULT_PERSON_TO_SELECT = "a3487e7a-2fb5-488c-b7ce-1a6395ed2453"; // frederic
const APP_STATUS_READY = "Ready...";
const APP_STATUS_BUSY = "Busy...";

const copyToOperatingSystemClipboard = (s) => {
    const el = document.createElement('textarea');
    el.value = s;
    document.body.appendChild(el);
    el.select();
    document.execCommand('copy');
    document.body.removeChild(el);
};

class GenealogyMainUI extends Component {

    state = {
        persons: [],
        selectedGuid: null,
        clipBoardPerson: null,
        applicationStatus: "Loading...",
    };

    componentDidMount() {

        this.reloadData();
    }

    trace(m) {

        console.log(`[GenealogyMainUI]${m}`);
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

            this.updateState("selectedGuid", null);
            return;
        }

        if (isObject(guid)) {

            guid = guid.value;
        }

        var newSelectedPerson = { ...this.getPersonFromGuid(guid) };
        if (newSelectedPerson) {

            this.trace(`handleChange ${__personDBClient.getPersonFullName(newSelectedPerson)}`);

            this.personHistory.push(newSelectedPerson.guid);
            this.trace(`personHistory  ${JSON.stringify(this.personHistory)}`);

            this.updateState("selectedGuid", guid);
        }
        else {
            this.userError(`Cannot select person guid:${guid}`);
        }
    };

    updateSelectedPerson = () => {

        const person = this.getPersonSelected();
        person.birthDate = stringDateToPersonDate(person.birthDate);
        person.deathDate = stringDateToPersonDate(person.deathDate);
        return __personDBClient.updatePerson(person); // Return promise
    }

    pasteSelectedPersonFromClipboardAs = (pasteAsEnum) => {

        if (this.state.clipBoardPerson === null) {

            this.userError('You must copy a person into the clipboard first');
        }
        else {

            this.trace(`Paste '${__personDBClient.getPersonFullName(this.state.clipBoardPerson)}' as ${pasteAsEnum} into '${__personDBClient.getPersonFullName(this.getPersonSelected())}`);
            const p = this.getPersonSelected();
            switch (pasteAsEnum) {

                case PASTE_OPERATION_AS.Father:
                    p.fatherGuid = this.state.clipBoardPerson.guid;
                    this.onUpdatePersonClick();
                    break;
                case PASTE_OPERATION_AS.Mother:
                    p.motherGuid = this.state.clipBoardPerson.guid;
                    this.onUpdatePersonClick();
                    break;
                case PASTE_OPERATION_AS.Spouse:
                    p.spouseGuid = this.state.clipBoardPerson.guid;
                    this.onUpdatePersonClick();
                    break;
                default:
                    throw new Error(`Invalid paste_as operation:${pasteAsEnum}`);
            }
        }
    }

    copySelectedPersonToClipboard = () => {

        const clipBoardPerson = { ...this.getPersonSelected() };
        const s = __personDBClient.getPersonFullName(clipBoardPerson);
        copyToOperatingSystemClipboard(s);
        this.trace(`Copied to clipboard person:${s}`);
        this.updateState("clipBoardPerson", clipBoardPerson);
    }

    goBackToPreviousPerson = () => {

        if (this.personHistory.length > 1) {

            var currentPersonGuid = this.personHistory.pop(); // eslint-disable-line
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

        if (this.state.selectedGuid)
            return this.getPersonFromGuid(this.state.selectedGuid);

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

        this.setAppStatus(APP_STATUS_BUSY);

        __personDBClient.loadPersons().then((persons) => {

            this.updateState('persons', persons, () => {

                this.selectDefaultPerson().then(() => {

                    this.setAppStatus(APP_STATUS_READY);
                });
            });
        });
    }

    onPersonUpdated = (person, callBack = undefined) => {

        this.trace(`onPersonUpdated person:${__personDBClient.getPersonFullName(person)}`);
        console.dir(person);
        var persons = this.state.persons;
        const index = persons.findIndex((p) => { return p.guid === person.guid; });
        if (index === -1) {
            this.userError(`Cannot find person in memory guid:${person.guid}, fullName:${__personDBClient.getPersonFullName(person)} `);

            return false;
        }
        else {
            persons[index] = person;
            this.updateState("persons", persons, callBack);
            return true;
        }
    };

    onPersonCreated = (person) => {

        this.trace(`onPersonCreated person:${__personDBClient.getPersonFullName(person)}`);
        var persons = this.state.persons;
        persons.push(person);
        this.updateState("persons", persons);

        return true;
    };

    onPersonDeleted = (person) => {

        person = this.getPersonFromGuid(person.guid); // Get the extact instance from the state
        this.trace(`onPersonDeleted person:${__personDBClient.getPersonFullName(person)}`);
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

        if (typeof (property) === 'string') {
            const newState = { ...this.state, [property]: value };
            // this.trace(`updateState property:${property}`);
            this.setState(newState, callBack);
        }

        //if (typeof (property) === 'object') {
        //    let newState = { ...this.state };
        //    Object.key(property).forEach((key) => {
        //        newState = { ...newState, [key]: property[key] };
        //    });
        //    console.log(`new state:${JSON.stringify(newState)}`);
        //    this.setState(newState);
        //}
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
    displayPersonSelectedInfo() {

        const p = this.getPersonSelected();
        this.trace(`Person Selected has ${p.images.length} images`);
    }
    deleteImage = (fileName) => {

        if (window.confirm(`Delete image ${fileName}?`)) {

            this.setAppStatus(APP_STATUS_BUSY);
            const person = this.getPersonSelected();

            __personDBClient.deleteImage(person, fileName).then((personUpdated) => {

                this.onPersonUpdated(personUpdated, () => {

                    this.displayPersonSelectedInfo();
                    this.setAppStatus(APP_STATUS_READY);
                });
            });
        }
    }
    getPersonImagesHtml(person) {

        if (!person.images)
            return [];

        return person.images.map((image) => {
            return (<span key={image.url}>

                <a target="top" href={image.url}>
                    <img key={image.url} alt={image.fileName} src={image.url} width={DEFAULT_IMAGE_WIDTH} />
                </a>

                <button type="button" className="btn btn-primary" onClick={() => {
                    this.deleteImage(image.fileName);
                }}> D </button>
            </span>);
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

        return (<tr>
            <td><label htmlFor={fieldName} className="col-sm-2 col-form-label">{fieldName}</label></td>
            <td><div className="col-sm-10">
                {jsx}
            </div></td>
        </tr>);
    }

    onFieldChange = (e, fieldName, isDate) => {

        const value = e.target.value;
        this.trace(`${fieldName} = ${value}`);

        const person = this.getPersonSelected();
        
        if (isDate) {
            const newDateValue = stringDateToPersonDate(value);
            this.trace(`isDate:${JSON.stringify(newDateValue)}`);
            person[fieldName] = newDateValue;
        }
        else {
            person[fieldName] = value;
        }

        this.onPersonUpdated(person);
    }

    getFieldRow(fieldName1, fieldValue1, fieldName2, fieldValue2, isMultiLine = false, isDate1 = false, isDate2 = false) {
        //if (isMultiLine)
        //    debugger;

        if (fieldValue1 && isPersonDate(fieldValue1)) {
            fieldValue1 = personDateToString(fieldValue1);
        }
        fieldValue1 = emptyStringOnNull(fieldValue1);


        if (fieldValue2 && isPersonDate(fieldValue2)) {
            fieldValue2 = personDateToString(fieldValue2);
        }
        fieldValue2 = emptyStringOnNull(fieldValue2);

        if (isMultiLine) {
            return (<tr>
                <td><label htmlFor={fieldName1} className="col-form-label">{fieldName1}</label></td>
                <td><div className="col-sm-10">
                    <textarea cols="80" rows="4" className="form-control-sm" id={fieldName1} value={fieldValue1}
                        onChange={(e) => { this.onFieldChange(e, fieldName1); }}
                    />
                </div></td>
            </tr>);
        }
        else {

            const getTDForLabel = (fieldName) => {
                return <td><label htmlFor={fieldName}>{fieldName} : </label></td>;
            };
            const getTDForInput = (fieldName, fieldValue, isDate) => {
                return (<td>
                    <div className="col-sm-10">
                        <input type="text" className="form-control-sm" id={fieldName} value={fieldValue}
                            onChange={(e) => { this.onFieldChange(e, fieldName, isDate); }}
                        />
                    </div>
                </td>);
            };
            const getSecondFieldRowHtml = () => {
                if (fieldName2) {
                    return null;
                }
                else return null;
            };

            return (<tr key={fieldName1}>
                {getTDForLabel(fieldName1)}
                {getTDForInput(fieldName1, fieldValue1, isDate1)}
                {fieldName2 && getTDForLabel(fieldName2)}
                {fieldName2 && getTDForInput(fieldName2, fieldValue2, isDate2)}
            </tr>);
        }
    }
    getPersonHtml(person) {
        return (
            <table border="0" cellPadding="1" cellSpacing="1" width="100%">
                <tbody>
                    {this.getFieldRow("lastName", person.lastName, "maidenName", person.maidenName)}
                    {this.getFieldRow("firstName", person.firstName, "middleName", replaceDash(person.middleName))}
                    {this.getFieldRow("birthDate", person.birthDate, "birthCity", person.birthCity, false, true)}
                    {this.getFieldRow("birthCountry", person.birthCountry, "deathDate", person.deathDate, false, false, true)}
                    {this.getFieldRow("deathCity", person.deathCity, "deathCountry", person.deathCountry)}                    
                    {this.getFieldRow("comment", person.comment, null, null, true)}

                    {this.isPersonHasSpouse(person) &&
                        this.getBlockRow("Spouse", this.getPersonSpouseSummaryHtml(person))}
                    {this.isPersonHasFather(person) &&
                        this.getBlockRow("Father", this.getPersonFatherSummaryHtml(person))}
                    {this.isPersonHasMother(person) &&
                        this.getBlockRow("Mother", this.getPersonMotherSummaryHtml(person))}
                    {this.hasPersonChildren(person) &&
                        this.getBlockRow("Children", this.getPersonChildrenSummaryHtml(person))}
                </tbody>
            </table>);
    }

    onUploadImageClick = () => {

        this.setAppStatus(APP_STATUS_BUSY);

        const input = document.querySelector('input[type="file"]');
        const fileToUpload = input.files[0];
        const formData = new FormData();
        formData.append('file', fileToUpload, fileToUpload.name);

        __personDBClient.uploadImage(this.getPersonSelected(), formData).then((person) => {

            this.onPersonUpdated(person, () => {

                this.displayPersonSelectedInfo();
                this.setAppStatus(APP_STATUS_READY);
            });
        });
    }
    onDeletePersonClick = () => {

        this.setAppStatus(APP_STATUS_BUSY);
        const person = this.getPersonSelected();
        if (window.confirm(`Delete ${__personDBClient.getPersonFullName(person)}`)) {

            __personDBClient.deletePerson(this.getPersonSelected()).then(() => {

                this.onPersonDeleted(person);
                this.selectDefaultPerson();
            }).finally(() => {
                this.setAppStatus(APP_STATUS_READY);
            });
        }
    }
    onCreatePersonClick = () => {

        this.setAppStatus(APP_STATUS_BUSY);
        __personDBClient.newPerson().then((newPerson) => {
            this.onPersonCreated(newPerson);
        }).finally(() => {
            this.setAppStatus(APP_STATUS_READY);
        });
    }
    onUpdatePersonClick = () => {
        this.setAppStatus(APP_STATUS_BUSY);
        this.updateSelectedPerson()
            .then((person) => {
                this.onPersonUpdated(person, () => {
                    this.setAppStatus(APP_STATUS_READY);
                });
            });
    }

    onKeyboardUpdate = (event) => {

        event.preventDefault();
        if (this.state.keyAltDown) {
            this.onUpdatePersonClick();
        }
    }

    onKeyboardCopyToClipboard = (event) => {

        event.preventDefault();
        if (this.state.keyAltDown) {
            this.copySelectedPersonToClipboard();
        }
    }

    onKeyboardAltKey = (event, down) => {

        if (this.state.keyAltDown !== down) {
            event.preventDefault();
            this.updateState('keyAltDown', down);
            this.trace(`ALT`);
        }
    }

    // https://developer.mozilla.org/en-US/docs/Web/API/KeyboardEvent/code/code_values     
    getKeyHandlers = () => {

        return (<span>

            <KeyHandler keyEventName={KEYUP} code="KeyU" onKeyHandle={this.onKeyboardUpdate} />
            <KeyHandler keyEventName={KEYUP} code="KeyC" onKeyHandle={this.onKeyboardCopyToClipboard} />

            <KeyHandler keyEventName={KEYDOWN} code="AltLeft" onKeyHandle={(event) => { this.onKeyboardAltKey(event, true); }} />
            <KeyHandler keyEventName={KEYUP} code="AltLeft" onKeyHandle={(event) => { this.onKeyboardAltKey(event, false); }} />
            <KeyHandler keyEventName={KEYDOWN} code="AltRight" onKeyHandle={(event) => { this.onKeyboardAltKey(event, true); }} />
            <KeyHandler keyEventName={KEYUP} code="AltRight" onKeyHandle={(event) => { this.onKeyboardAltKey(event, false); }} />
        </span>);
    }

    askForUsernameAndPassword() {

        var userName = prompt("Username?", DEFAULT_USER_NAME);
        var password = prompt("Password?");
        __personDBClient.setUsernamePassword(userName, password);
    }

    render() {
        const personSelected = this.getPersonSelected();
        let selectionForComboBox = null;

        const personSelectedFullName = __personDBClient.getPersonFullName(personSelected);

        let imageCount = 0;
        if (personSelected && personSelected.images)
            imageCount = personSelected.images.length;

        if (personSelected) {

            selectionForComboBox = {
                value: personSelected.guid,
                label: __personDBClient.getPersonFullName(personSelected)
            };
        }

        return (
            <React.Fragment>

                {this.getKeyHandlers()}

                <div>
                    <h2>MyGenealogy</h2>

                    <Alert variant={this.state.applicationStatus === APP_STATUS_BUSY ? 'danger' : 'primary'}>
                        {this.state.applicationStatus}
                    </Alert>

                    <table>
                        <tbody>
                            <tr>
                                <td><button type="button" className="btn btn-primary" onClick={() => { this.goBackToPreviousPerson(); }}> Back </button> &nbsp;</td>
                                <td>
                                    <ButtonGroup aria-label="Basic example">

                                        <Button variant="secondary" onClick={() => { this.copySelectedPersonToClipboard(); }}> Copy To Clipboard </Button>
                                        <Button variant="secondary" onClick={this.onUpdatePersonClick}> Update </Button>
                                        <Button variant="secondary" onClick={this.onCreatePersonClick}> New </Button>
                                        <Button variant="secondary" onClick={this.onDeletePersonClick}> Delete </Button>
                                        <Button variant="secondary" onClick={this.onUploadImageClick}> Upload Image </Button>
                                    </ButtonGroup>
                                    <input type="file" name="file" />
                                </td>

                                <td>
                                    <Dropdown>
                                        <Dropdown.Toggle variant="success" id="dropdown-basic">
                                            Relationships - {__personDBClient.getPersonFullName(this.state.clipBoardPerson)}
                                        </Dropdown.Toggle>

                                        <Dropdown.Menu>
                                            <Dropdown.Item onClick={() => { this.pasteSelectedPersonFromClipboardAs(PASTE_OPERATION_AS.Father); }} >Paste as Father </Dropdown.Item>
                                            <Dropdown.Item onClick={() => { this.pasteSelectedPersonFromClipboardAs(PASTE_OPERATION_AS.Mother); }} >Paste as Mother </Dropdown.Item>
                                            <Dropdown.Item onClick={() => { this.pasteSelectedPersonFromClipboardAs(PASTE_OPERATION_AS.Spouse); }} >Paste as Spouse </Dropdown.Item>
                                        </Dropdown.Menu>
                                    </Dropdown>
                                </td>



                                <td>
                                    <Dropdown>
                                        <Dropdown.Toggle variant="success" id="dropdown-basic">
                                            Maintenance
                                        </Dropdown.Toggle>

                                        <Dropdown.Menu>
                                            <Dropdown.Item onClick={
                                                this.askForUsernameAndPassword
                                            } >Sign in</Dropdown.Item>
                                        </Dropdown.Menu>
                                    </Dropdown>
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
                    {personSelected && <a target="top" href={`https://mygenealogie.blob.core.windows.net/person-db/${personSelected.guid}.json`}>Json Resource</a>}
                    {personSelected && this.getPersonHtml(personSelected)}
                    {personSelected && this.getPersonImagesHtml(personSelected)}
                    {this.GetPersonsSelector()}
                </div>
            </React.Fragment>
        );
    }
}

export default connect()(GenealogyMainUI);

