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

class Home extends Component {
    
    state = {
        persons: [],
        selectedOption: null
    };

    componentDidMount() {

        this.reloadData();
    }

    handleChange = (selectedOption) => {
        console.log(`handleChange  ${JSON.stringify(selectedOption)}`);
        this.updateState("selectedOption", selectedOption);
        console.log(`Option selected:`, selectedOption);
    };


    selectSpouse = () => {

        var spouse = this.getSpouseForPersonSelected(this.getPersonSelected());
        const option = { value: spouse.guid, label: this.getPersonFullName(spouse) };
        this.handleChange(option);
    }
       
    getPersonFullName(p) {

        const maidenName = p.maidenName ? ` [${p.maidenName}]` : ``;
        const middleName = replaceDash(p.middleName ? `${p.middleName}` : ``);
        const firstName = replaceDash(p.firstName);
        return `${p.lastName}${maidenName}, ${firstName}${middleName}`;
    }

    getPersonFromGuid(guid) {

        var c = this.state.persons.find((p) => p.guid === guid);
        console.dir(c);
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

    getSpouseForPersonSelected(personSelected) {

        var c = this.state.persons.find((p) => p.spouseGuid === personSelected.guid);
        return c;
    }

    reloadData = () => {

        return fetch('api/MyGenealogie/GetPersons').then(response => response.json())
            .then(data => {
                console.log(`reloadData data:${JSON.stringify(data)}`);
                this.updateState('persons', data);
            });
    }

    updateState = (property, value) => {

        this.setState({ ...this.state, [property]: value });
    }

    GetPersonsSelector() {

        var r = this.state.persons.map((p) => {
            return (<li key={p.guid}>
                {p.lastName}{p.maidenName ? ` [${p.maidenName}]` : ``}, {p.firstName} {p.middleName ? `, ${p.middleName}` : ``}
            </li>);
        });
        return r;
    }

    GetPersonsDataForCombo() {
        var r = this.state.persons.map((p) => {
            return {
                value: p.guid,
                label: `${p.lastName}${p.maidenName ? `[${ p.maidenName }]` : ``} - ${p.firstName}`
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
            return this.getPersonFullName(spouse);
        }
        return null;
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
                    <label htmlFor="txtComment">Comment</label>
                    <textarea cols="80" rows="4"  className="form-control-sm" id="txtComment" value={emptyStringOnNull(person.comment)} / >
                </div>
                {this.isPersonHasSpouse(person) && (<div>
                    {this.getPersonSpouseSummaryHtml(person)}
                    <button type="button" class="btn btn-primary" onClick={this.selectSpouse}> View </button>
                </div>)}
            </form>
            );
    }

    render() {
        const personSelected = this.getPersonSelected();
        return (
            <div>
                <h1>My Genealogie</h1>
                <button type="button" class="btn btn-primary" onClick={this.reloadData}> RELOAD </button>
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
