import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Link } from 'react-router-dom';

// https://emotion.sh/docs/introduction
// https://github.com/JedWatson/react-select
import Select from 'react-select';


function emptyStringOnNull(v) {
    if (v === null || v === undefined)
        return '';
    return v;
};

function replaceDash(s) {

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

class Home extends Component {
    
    state = {
        persons: [],
        selectedOption: null
    };

    componentDidMount() {

        this.reloadData();
    }

    personHistory = []; // guid list of all person viewed

    handleChange = (selectedOption) => {

        console.log(`handleChange  ${JSON.stringify(selectedOption)}`);
        this.personHistory.push(selectedOption.value); // add guid
        this.updateState("selectedOption", selectedOption);
        console.log(`personHistory  ${JSON.stringify(this.personHistory)}`);
    };

    goBackToPreviousPerson = () => {
        
        if (this.personHistory.length > 1) {
            var currentPersonGuid = this.personHistory.pop();
            var previousPersonGuid = this.personHistory.pop();
            this.selectPerson(previousPersonGuid);
        }
    }

    selectPerson = (guid) => {

        const person = this.getPersonFromGuid(guid);
        console.dir(person);
        const option = { value: person.guid, label: this.getPersonFullName(person) };
        this.handleChange(option);
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

        if (this.state.selectedOption) {

            var guid = this.state.selectedOption.value;
            return this.getPersonFromGuid(guid);
        }
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
                this.updateState('persons', data);
            });
    }

    updateState = (property, value) => {

        this.setState({ ...this.state, [property]: value });
    }

    GetPersonsSelector() {

        var r = this.state.persons.map((p) => {
            return (<li key={p.guid}>
                {this.getPersonFullName(p)}
            </li>);
        });
        return r;
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
            return <img src={image.url} width={150} />;
        });
    }

    getPersonSpouseSummaryHtml(person) {

        if (this.isPersonHasSpouse(person)) {
            const spouse = this.getSpouseForPersonSelected(person);
            return (<span>
                <button type="button" className="btn btn-primary" onClick={() => { this.selectPerson(this.getSpouseForPersonSelected(person).guid); }}> View </button> - 
                {this.getPersonFullName(spouse)}
            </span> );
        }
        return null;
    }

    getPersonFatherSummaryHtml(person) {
        if (this.isPersonHasFather(person)) {
            const father = this.getFatherForPersonSelected(person);
            return (<span>
                <button type="button" className="btn btn-primary" onClick={() => { this.selectPerson(this.getFatherForPersonSelected(person).guid); }}> View </button> -
                {this.getPersonFullName(father)}
            </span>);
        }
        return null;
    }

    getPersonMotherSummaryHtml(person) {
        if (this.isPersonHasMother(person)) {
            const mother = this.getMotherForPersonSelected(person);
            return (<span>
                <button type="button" className="btn btn-primary" onClick={() => { this.selectPerson(this.getMotherForPersonSelected(person).guid); }}> View </button> -
                {this.getPersonFullName(mother)}
            </span>);
        }
        return null;
    }

    hasPersonChildren(person) {

        const children = this.getChildrenForPersonSelected(person);
        return children.length > 0;
    }

    getPersonChildrenSummaryHtml(person) {

        const children = this.getChildrenForPersonSelected(person);
        const childrenHtml = children.map((c) => {
            return (<li>
                <button type="button" className="btn btn-primary" onClick={() => { this.selectPerson(c.guid); }} > View </button> - 
                {this.getPersonFullName(c)}
            </li>);
        });
        return <ul>{childrenHtml}</ul>;
    }

    getPersonHtml(person) {

        return (
            <form>
                <div className="form-group">
                    <label htmlFor="txtLastName">LastName </label>
                    <input type="text" className="form-control-sm" id="txtLastName" value={person.lastName} />
                </div>
                <div className="form-group">
                    <label htmlFor="txtMaidenName">MaidenName</label>
                    <input type="text" className="form-control-sm" id="txtMaidenName" value={emptyStringOnNull(person.maidenName)} />
                </div>
                <div className="form-group">
                    <label htmlFor="txtFirstName">FirstName</label>
                    <input type="text" className="form-control-sm" id="txtFirstName" value={emptyStringOnNull(person.firstName)} />
                </div>
                <div className="form-group">
                    <label htmlFor="txtMiddleName">MiddleName</label>
                    <input type="text" className="form-control-sm" id="txtFirstName" value={emptyStringOnNull(person.middleName)} />
                </div>
                
                <div className="form-group">
                    <label htmlFor="txtBirthDate">BirthDate</label>
                    <input type="text" className="form-control-sm" id="txtBirthDate" value={formatYear(person.birthDate)} />
                </div>
                <div className="form-group">
                    <label htmlFor="txtDeathDate">DeathDate</label>
                    <input type="text" className="form-control-sm" id="txtDeathDate" value={formatYear(person.deathDate)} />
                </div>

                <div className="form-group">
                    <label htmlFor="txtComment">Comment</label>
                    <textarea cols="80" rows="4"  className="form-control-sm" id="txtComment" value={emptyStringOnNull(person.comment)} / >
                </div>
                {this.isPersonHasSpouse(person) && (<div>
                    <b>Spouse</b>: {this.getPersonSpouseSummaryHtml(person)}
                </div>)}

                {this.isPersonHasFather(person) && (<div>
                    <b>Father</b>: {this.getPersonFatherSummaryHtml(person)}
                </div>)}

                {this.isPersonHasMother(person) && (<div>
                    <b>Mother</b>: {this.getPersonMotherSummaryHtml(person)}
                </div>)}

                {this.hasPersonChildren(person) && (<div>
                    <b>Children</b>: {this.getPersonChildrenSummaryHtml(person)}
                </div>)}
            </form>
            );
    }

    render() {
        const personSelected = this.getPersonSelected();
        return (
            <div>
                <h1>My Genealogie</h1>
                <button type="button" className="btn btn-primary" onClick={
                    () => { this.goBackToPreviousPerson(); }
                }> Back </button>
                <hr />

                <Select
                    isClearable={true} isSearchable={true}
                    value={this.state.selectedOption}
                    onChange={this.handleChange}
                    options={this.GetPersonsDataForCombo()}
                />

                <hr />
                {personSelected && this.getPersonHtml(personSelected)}
                <hr />
                {personSelected && this.getPersonImagesHtml(personSelected)}

                <ul>
                    {this.GetPersonsSelector()}
                </ul>
            </div>
        );
    }
}

export default connect()(Home);

