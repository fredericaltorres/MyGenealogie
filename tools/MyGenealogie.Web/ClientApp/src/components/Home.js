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

class Home extends Component {
    
    state = {
        persons: [],
        selectedOption: null
    };



    componentDidMount() {

        this.reloadData();
    }

    handleChange = (selectedOption) => {

        this.updateState("selectedOption", selectedOption);
        console.log(`Option selected:`, selectedOption);
    };

    getPersonSelected() {

        if (this.state.selectedOption) {

            var guid = this.state.selectedOption.value;
            var c = this.state.persons.find((p) => p.guid === guid);
            console.dir(c);
            return c;
        }
        return null;
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
            return <img src={image.url} />;
        });
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
