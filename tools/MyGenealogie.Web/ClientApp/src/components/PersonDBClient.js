import isObject from 'lodash/isObject';
import isUndefined from 'lodash/isUndefined';

const PERSON_TO_DATE_NULL = { year: 0, month: 0, day: 0 };

export const PASTE_OPERATION_AS = {
    Mother: 'Mother',
    Father: 'Father',
    Spouse: 'Spouse'
};

export function isPersonDate(d) {

    return isObject(d) && !isUndefined(d.year);
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
        year:  (parts[0] === undefined || parts[0] === '') ? 0 : parseInt(parts[0]),
        month: (parts[1] === undefined || parts[1] === '')  ? 0 : parseInt(parts[1]),
        day:   (parts[2] === undefined || parts[20] === '')  ? 0 : parseInt(parts[2])
    };
}

const JSON_CONTENT_TYPE = { 'Content-Type': 'application/json' };

export class PersonDBClient {

    _userName = null;
    _password = null;

    getHeaders() {

        var headers = { ...JSON_CONTENT_TYPE };
        if (this._userName)
            headers['Username'] = this._userName;
        if (this._password)
            headers['Password'] = this._password;
        return headers;
    }
    setUsernamePassword(userName, password) {

        this.trace(`setUsernamePassword userName:${userName}, password:*****`);
        this._userName = userName;
        this._password = password;
    }
    trace(m) {

        console.log(`[PersonDBClient]${m}`);
    }
    __buildUrl(urlAction, guid = null) {
        if(guid)
            return `api/MyGenealogie/${urlAction}/${guid}`;
        else 
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

        if (!response.ok) {
            response.json()
                .then((error) => {
                    const m = `http call failed:${response.status} - ${error.message}`;
                    console.log(m);
                    alert(`ERROR:${m}`);
                });
        }
        return response;
    }
    __buildFetchBlock(method, data) {

        if (method === 'GET')
            return {
                method,
                headers: this.getHeaders()
            };

        return {
            method,
            headers: this.getHeaders(),
            body: JSON.stringify(data)
        };
    }
    __buildFetchBlockForImages(method, files, person) {

        const h = this.getHeaders();
        h.enctype = "multipart/form-data";
        delete h['Content-Type'];
        h.guid = person.guid;
        return {
            method,
            headers: h,
            body: files
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

    // https://programmingwithmosh.com/javascript/react-file-upload-proper-server-side-nodejs-easy/
    uploadImage(person, imageData) {

        return new Promise((resolve, reject) => {

            this.trace(`Upload image for person ${this.getPersonFullName(person)}`);
            return fetch(this.__buildUrl('UploadImage'), this.__buildFetchBlockForImages('POST', imageData, person))
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

    deleteImage(imageFileName, person) {

        return new Promise((resolve, reject) => {

            this.trace(`Delete image:${imageFileName} for person ${this.getPersonFullName(person)}`);
            ++++
            return fetch(this.__buildUrl('DeleteImage'), this.__buildFetchBlock('DELETE', {}, person))
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
