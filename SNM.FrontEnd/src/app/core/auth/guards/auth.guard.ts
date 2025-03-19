import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';

@Injectable()

export class AuthGuard implements CanActivate {

  constructor(private router: Router) {}

  canActivate(): boolean {
    // Vérifiez si le jeton d'accès est présent dans le local storage
    const accessToken = localStorage.getItem('accessToken');

    if (accessToken) {
      // L'utilisateur est connecté, autorisez la navigation
      return true;
    }

    // L'utilisateur n'est pas connecté, redirigez vers la page de connexion
    this.router.navigate(['/sign-in']);
    return false;
  }
}
