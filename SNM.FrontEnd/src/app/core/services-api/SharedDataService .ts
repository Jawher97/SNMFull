// SharedDataService
import { Injectable, EventEmitter } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class SharedDataService {
  private selectedPostSubject = new BehaviorSubject<any>(null);
  selectedPost$ = this.selectedPostSubject.asObservable();

  updateSelectedPost(value: any) {
    this.selectedPostSubject.next(value);
  
  }
  getSelectedPostObservable(): Observable<any> {
    return this.selectedPostSubject.asObservable();
  }
}
