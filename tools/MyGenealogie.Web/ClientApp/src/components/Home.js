import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Link } from 'react-router-dom';
import isObject from 'lodash/isObject';

// https://emotion.sh/docs/introduction
// https://github.com/JedWatson/react-select
import Select from 'react-select';

function isPersonDate(d) {
    return isObject(d) && d.year;
}

function emptyStringOnNull(v) {
    if (v === null || v === undefined)
        return '';
    return v;
};

function replaceDash(s) {
    if (!s)
        return s;
    return s.replace(new RegExp('-', 'g'), ' ');
}

function formatYear(d) {
    if (!d) return '';
    if (d.year === 0 && d.month === 0 && d.day === 0) return '';
    if (d.year !== 0 && d.month === 0 && d.day === 0) return `${d.year}`;
    if (d.year !== 0 && d.month !== 0 && d.day === 0) return `${d.year}-${d.month}`;
    if (d.year !== 0 && d.month !== 0 && d.day !== 0) return `${d.year}-${d.month}-${d.day}`;
    return 'date-issue';
}

function stringDateToPersonDate(d) {
    var parts = d.split('-');
    return {
        year: parts[0] === undefined ? 0 : parseInt(parts[0]),
        month: parts[1] === undefined ? 0 : parseInt(parts[1]),
        day: parts[2] === undefined ? 0 : parseInt(parts[2])
    };
}

const DEFAULT_IMAGE_WIDTH = 150;
const DEFAULT_PERSON_TO_SELECT = "eb6db547-abee-42ec-89c3-da273f8e30f3";

class Home extends Component {

    state = {
        persons: [],        
        selectedPerson: null,
    };

    componentDidMount() {

        this.reloadData();
    }

    personHistory = []; // guid list of all person viewed

    userError(errorMessage) {
        alert(`ERROR:${errorMessage}`);
    }

    handleChange = (guid) => {
        if (guid === null) {
            this.updateState("selectedPerson", null);
            return;
        }

        if (isObject(guid)) { // Coming from react-select combobox
            guid = guid.value;
        }

        var selectedPerson = { ...this.getPersonFromGuid(guid) }; // Make a physical copy
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
        alert(`UPDATE ${JSON.stringify(person)}`);
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

    getPersonFullName(p) {

        const maidenName = p.maidenName ? ` [${p.maidenName}]` : ``;
        const middleName = replaceDash(p.middleName ? ` ${p.middleName}` : ``);
        const firstName = replaceDash(p.firstName);
        return `${p.lastName}${maidenName}, ${firstName}${middleName} - ${p.guid}`;
    }

    getPersonFromGuid(guid) {

        var c = this.state.persons.find((p) => p.guid === guid);
        return c;
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

        const c = this.state.persons.find((p) => p.guid === personSelected.fatherGuid);
        return c;
    }

    getMotherForPersonSelected(personSelected) {

        return this.state.persons.find((p) => p.guid === personSelected.motherGuid);
    }

    getChildrenForPersonSelected(personSelected) {

        var children = this.state.persons.filter((p) => p.fatherGuid === personSelected.guid || p.motherGuid === personSelected.guid);
        return children;
    }

    reloadData = () => {

        return fetch('api/MyGenealogie/GetPersons').then(response => response.json())
            .then(data => {
                // console.log(`reloadData data:${JSON.stringify(data)}`);
                this.updateState('persons', data, () => {

                    if (DEFAULT_PERSON_TO_SELECT) {                        
                        this.selectPerson(DEFAULT_PERSON_TO_SELECT);
                    }
                });
            });
    }

    updateState = (property, value, callBack = () => { }) => {

        if(typeof(property)==='string')
            this.setState({ ...this.state, [property]: value }, callBack);

        if (typeof (property) === 'object') {
            let newState = { ...this.state };
            Object.key(property).forEach((key) => {
                newState = { ...newState, [key]: property[key]};
            });
            console.log(`new state:${JSON.stringify(newState)}`);
            this.setState(newState, callBack);
        }
    }

    GetPersonsSelector() {

        var r = this.state.persons.map((p) => {
            return (<li key={p.guid}>
                {this.getPersonFullName(p)}
            </li>);
        });
        return <ul>{r}</ul>;
    }

    GetPersonsDataForCombo() {
        var r = this.state.persons.map((p) => {
            return {
                value: p.guid,
                label: this.getPersonFullName(p)
            };
        });
        return r;
    }

    getPersonImagesHtml(person) {

        return person.images.map((image) => {
            return <img key={image.url} src={image.url} width={DEFAULT_IMAGE_WIDTH} />;
        });
    }

    getPersonSpouseSummaryHtml(person) {

        if (this.isPersonHasSpouse(person)) {
            const spouse = this.getSpouseForPersonSelected(person);
            return (<span>
                <button type="button" className="btn btn-primary" onClick={() => { this.selectPerson(this.getSpouseForPersonSelected(person).guid); }}> View </button> 
                &nbsp;{this.getPersonFullName(spouse)}
            </span> );
        }
        return null;
    }

    getPersonFatherSummaryHtml(person) {

        if (this.isPersonHasFather(person)) {
            const father = this.getFatherForPersonSelected(person);
            return (<span>
                <button type="button" className="btn btn-primary" onClick={() => { this.selectPerson(this.getFatherForPersonSelected(person).guid); }}> View </button> 
                &nbsp;{this.getPersonFullName(father)}
            </span>);
        }
        return null;
    }

    getPersonMotherSummaryHtml(person) {

        if (this.isPersonHasMother(person)) {
            const mother = this.getMotherForPersonSelected(person);
            return (<span>
                <button type="button" className="btn btn-primary" onClick={() => { this.selectPerson(this.getMotherForPersonSelected(person).guid); }}> View </button>
                &nbsp;{this.getPersonFullName(mother)}
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
                &nbsp;{this.getPersonFullName(c)}
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
        const newState = { ...this.state, selectedPerson};
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

            <button type="button" className="btn btn-primary" onClick={this.updateSelectedPerson}> Update </button> 
        </form>);
    }

    render() {
        let selectionForComboBox = null;
        const personSelected = this.getPersonSelected();

        if (personSelected) {

            selectionForComboBox = {
                value: personSelected.guid,
                label: this.getPersonFullName(personSelected)
            };
        }
                
        return (
            <div>
                <h2>MyGenealogie</h2>
                <button type="button" className="btn btn-primary" onClick={() => { this.goBackToPreviousPerson(); }}> Back </button>
                <Select
                    isClearable={true} isSearchable={true}
                    value={selectionForComboBox}
                    onChange={this.handleChange}
                    options={this.GetPersonsDataForCombo()}
                />
                <hr/>
                {personSelected && this.getPersonHtml(personSelected)}              
                {personSelected && this.getPersonImagesHtml(personSelected)}

                {this.GetPersonsSelector()}
            </div>
        );
    }
}

export default connect()(Home);

