import isObject from 'lodash/isObject';

const PERSON_TO_DATE_NULL = { year: 0, month: 0, day: 0 };

export const PASTE_OPERATION_AS = {
    Mother: 'Mother',
    Father: 'Father',
    Child: 'Child',
    Spouse: 'Spouse'
};

export function isPersonDate(d) {

    return isObject(d) && d.year;
}

export function emptyStringOnNull(v) {

    if (v === null || v === undefined)
        return '';

    return v;
};

export function replaceDash(s) {

    if (!s)
        return s;

    return s.replace(new RegExp('-', 'g'), ' ');
}

export function personDateToString(d) {

    if (!d) return '';
    if (d.year === 0 && d.month === 0 && d.day === 0) return '';
    if (d.year !== 0 && d.month === 0 && d.day === 0) return `${d.year}`;
    if (d.year !== 0 && d.month !== 0 && d.day === 0) return `${d.year}-${d.month}`;
    if (d.year !== 0 && d.month !== 0 && d.day !== 0) return `${d.year}-${d.month}-${d.day}`;

    return 'date-issue';
}

export function stringDateToPersonDate(d) {

    if (isPersonDate(d)) return d; // If the field was never updated it is still an object
    if (!d) return PERSON_TO_DATE_NULL;
    
    var parts = d.split('-');

    return {
        year: parts[0] === undefined ? 0 : parseInt(parts[0]),
        month: parts[1] === undefined ? 0 : parseInt(parts[1]),
        day: parts[2] === undefined ? 0 : parseInt(parts[2])
    };
}

const JSON_CONTENT_TYPE = { 'Content-Type': 'application/json' };

export class PersonDBClient {

    trace(m) {

        console.log(`[PersonDBClient]${m}`);
    }
    __buildUrl(urlAction) {

        return `api/MyGenealogie/${urlAction}`;
    }
    getPersonFullName(p) {

        if (!p)
            return null;

        const maidenName = p.maidenName ? ` [${p.maidenName}]` : ``;
        const middleName = replaceDash(p.middleName ? ` ${p.middleName}` : ``);
        const firstName = replaceDash(p.firstName);

        return `${p.lastName}${maidenName}, ${firstName}${middleName} - ${p.guid}`;
    }
    handleErrors(response) {

        if (!response.ok) 
            throw Error(response.statusText);

        return response;
    }
    __buildFetchBlock(method, data) {

        if (method === 'GET')
            return {
                method,
                headers: JSON_CONTENT_TYPE,
            };

        return {
            method,
            headers: JSON_CONTENT_TYPE,
            body: JSON.stringify(data)
        };
    }
    loadPersons() {

        return new Promise((resolve, reject) => {
            
            this.trace(`Load persons`);
            return fetch(this.__buildUrl('GetPersons'), this.__buildFetchBlock('GET', ''))
                .then(this.handleErrors)
                .then(response => {
                    response.json().then((persons) => {
                        resolve(persons);
                    });
                })
                .catch(function (error) {
                    console.log(error);
                    reject(error);
                });
        });
    }
    newPerson() {

        return new Promise((resolve, reject) => {

            this.trace(`New person`);
            return fetch(this.__buildUrl('NewPerson'), this.__buildFetchBlock('POST', ''))
            .then(this.handleErrors)
            .then(response => {
                response.text().then((personJson) => {
                    const person = JSON.parse(personJson);
                    console.log(`New person:${this.getPersonFullName(person)}`);
                    resolve(person);
                });
            })
            .catch(function (error) {
                console.log(error);
                reject(error);
            });
        });
    }
    deletePerson(person) {
        
        return new Promise((resolve, reject) => {
            
            this.trace(`delete person ${this.getPersonFullName(person)}`);
            return fetch(this.__buildUrl('DeletePerson'), this.__buildFetchBlock('DELETE', person.guid))
            .then(this.handleErrors)
            .then(response => {
                resolve(true);
            })
            .catch(function (error) {
                console.log(error);
                reject(error);
            });
        });
    }
    updatePerson(person) {

        return new Promise((resolve, reject) => {

            this.trace(`Update person ${this.getPersonFullName(person)}`);
            return fetch(this.__buildUrl('UpdatePerson'), this.__buildFetchBlock('PUT', person))
            .then(this.handleErrors)
            .then(response => {
                if (response.ok)
                    resolve(person);
                else
                    reject(person);
            })
            .catch(function (error) {
                console.log(error);
                reject(error);
            });
        });
    }

    updatePersonApi = (person) => {

        console.log(`Call to back end to update person:${__personDBClient.getPersonFullName(person)}`);

        return fetch('api/MyGenealogie/UpdatePerson', {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(person) // body data type must match "Content-Type" header
        })
            .then(response => {
                console.log(response);
                console.log(`back end update Ok:${response.ok} person:${__personDBClient.getPersonFullName(person)}`);
                this.onPersonUpdated(person);
            });
    }
}

const __personDBClient = new PersonDBClient();
