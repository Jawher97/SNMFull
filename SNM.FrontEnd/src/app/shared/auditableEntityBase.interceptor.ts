import { Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from 'app/core/auth/auth.service';
import { UserService } from 'app/core/user/user.service';
import { User } from 'app/core/user/user.types';

@Injectable()
export class AuditableEntityBaseInterceptor implements HttpInterceptor
{
    
    /**
     * Constructor
     */
    constructor(private _authService: AuthService, private _userService: UserService)
    {
    }
    currentUser: User;
    /**
     * Intercept
     *
     * @param req
     * @param next
     */
    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        // Check if the request is a POST, PUT, or DELETE
        if (['POST', 'PUT', 'DELETE'].includes(request.method)) {
              // Get the user's name
        this._userService.user$.subscribe((user) => {
            this.currentUser = user;
        });

            // Set common properties on the request body if a user is authenticated
            if (this.currentUser) {
                // const body = request.body as EntityModel; // Assuming the body is of type EntityModel
                const body = request.body; // Assuming the body is of type EntityModel
                if (body) {
                     if (['POST'].includes(request.method)) {
                         body.createdBy = this.currentUser.id;
                         body.lastModifiedBy =  this.currentUser.id;
                         body.createdOn = new Date();
                     }
                     else if (['PUT'].includes(request.method)) {
                        body.lastModifiedBy =  this.currentUser.id;
                        body.lastModifiedOn = new Date();
                     }
                     else if (['DELETE'].includes(request.method)) {
                         body.deletedBy = this.currentUser.id;
                         body.deletedOn = new Date();
                     }

                }
            }
        }

        return next.handle(request);
    }
}
