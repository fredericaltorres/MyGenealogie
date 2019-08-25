import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Link } from 'react-router-dom';

class Home extends Component {
    
    state = {
        persons : []
    };

    componentDidMount() {
        this.reloadData();
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
                {p.lastName} {p.maidenName ? ` [${p.maidenName}]` : ``} - {p.firstName} {p.middleName ? ` - ${p.middleName}` : ``}
            </li>);
        });
        return r;
    }

    render() {
        return (
            <div>
                <h1>Hello, world!</h1>
                someting
                <button onClick={this.reloadData}> RELOAD </button>
                <hr />
                <ul>
                    {this.GetPersonsSelector()}
                </ul>
            </div>
        );
    }
}

export default connect()(Home);
