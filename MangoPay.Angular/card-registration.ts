import { Observable, throwError } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, concatMap, map } from 'rxjs/operators';

export class Card {
    cardNumber?: string;
    cardExpirationDate: string;
    cardCvx: string;
}
export class Registration {
    accessKey: string;
    preregistrationData: string;
    registrationData?: string;
    cardRegistrationURL: string;
}
export class cardRegistrationService {
    constructor(private http: HttpClient) { }

    registerNewCard(card: Card): Observable<boolean> {
        // 1.- Obtain Card Registration Request Data
        const url = `apiUrl/cardRegistrationsAction`;

        return this.http.post<Registration>(url, card, {}).pipe(
            concatMap((registration: Registration) => {
                let body = new URLSearchParams();
                body.set('accessKeyRef', registration.accessKey);
                body.set('data', registration.preregistrationData);
                body.set('cardNumber', card.cardNumber);
                body.set('cardExpirationDate', card.cardExpirationDate.toString());
                body.set('cardCvx', card.cardCvx);

                // 2.- Call Tokenization Service to Obtain Registration Data
                return this.http
                    .post(registration.cardRegistrationURL, body.toString(), {
                        headers: new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded'),
                        responseType: 'text'
                    })
                    .pipe(
                        concatMap((response: any) => {
                            registration.registrationData = response;

                            // 3.- Send registration object with the retrieved data:
                            const updateUrl = `apiUrl/updateRegistrationDataAction`;
                            return this.http.put(updateUrl, registration).pipe(
                                map((success: boolean) => {
                                    return success;
                                }),
                                catchError(this.handleError)
                            );
                        })
                    );
            })
        );
    }
    private handleError(err) {
        let errorMessage: string;
        if (err.error instanceof ErrorEvent) {
            errorMessage = `An error occurred: ${err.error.message}`;
        } else {
            errorMessage = `Backend returned code ${err.status}: ${err.body.error}`;
        }
        return throwError(errorMessage);
    }
}