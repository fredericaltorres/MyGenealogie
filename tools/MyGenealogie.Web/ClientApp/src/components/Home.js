import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Link } from 'react-router-dom';

// https://emotion.sh/docs/introduction
// https://github.com/JedWatson/react-select
import Select from 'react-select';

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
                {p.lastName} {p.maidenName ? ` [${p.maidenName}]` : ``} - {p.firstName} {p.middleName ? ` - ${p.middleName}` : ``}
            </li>);
        });
        return r;
    }

    GetPersonsDataForCombo() {
        var r = this.state.persons.map((p) => {
            return {
                value: p.puid,
                label: `${p.lastName} - ${p.firstName}`
            };
        });
        return r;
    }

    render() {
        return (
            <div>
                <h1>My Genealogie</h1>
                <button onClick={this.reloadData}> RELOAD </button>
                <hr />

                <Select
                    value={this.state.selectedOption}
                    onChange={this.handleChange}
                    options={this.GetPersonsDataForCombo()}
                />

                <hr />

                <ul>
                    {this.GetPersonsSelector()}
                </ul>
            </div>
        );
    }
}

export default connect()(Home);
