import { Component } from "@angular/core";
import { LinkedInService } from "app/core/services-api/linkedin.service";
import { environment, linkedInPaths } from "environments/environment.development";

const linkedInOAuth = `${linkedInPaths.linkedinOAuthURL}`;

@Component({
  selector: "linkedIn-login",
  templateUrl: "./login.component.html",
  styleUrls: ["./login.component.scss"]
})

export class LoginComponent {

  connected: boolean = false;


  constructor(private linkedinservice: LinkedInService) {
      // Vérifiez si le token LinkedIn est présent dans le localStorage
      const linkedInToken = localStorage.getItem('LinkedIn access_token');
  
    // Mettez à jour la variable connected en fonction de la présence du token
    this.connected = !!linkedInToken;
  }
  
  login() 
  {
    this.linkedinservice.login();
    
    this.connected = true

    
  }


  logOut(): void {
    localStorage.removeItem('LinkedIn access_token');
    localStorage.removeItem('LinkedIn refresh_token');
    this.connected=false
  }
}
