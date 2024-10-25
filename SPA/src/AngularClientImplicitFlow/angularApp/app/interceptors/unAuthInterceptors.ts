import {HttpErrorResponse, HttpHandler, HttpInterceptor, HttpRequest, HttpResponse} from "@angular/common/http";
import {finalize, tap} from "rxjs/operators";
import {Injectable} from "@angular/core";
import {Router} from "@angular/router";

@Injectable()
export class UnAuthInterceptor implements HttpInterceptor {
    constructor(private router: Router) {}

    intercept(req: HttpRequest<any>, next: HttpHandler) {

        // extend server response observable with logging
        return next.handle(req)
            .pipe(
                tap(
                    () => {},
                    (err) => {
                        if (err instanceof HttpErrorResponse) {
                            if (err.status == 401) {
                                this.router.navigate([`unauthorized`])
                            }
                        }
                    }
                )
            );
    }
}