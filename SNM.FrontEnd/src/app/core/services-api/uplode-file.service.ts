import { HttpClient, HttpErrorResponse, HttpEventType } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class UplodeFileService {
  // Private
  private brandPath: string;
  progress: number;
  message: string;
  // @Output() public onUploadFinished = new EventEmitter();
  /**
     * Constructor
     */
  constructor(private _httpClient: HttpClient) {
    // Set the private defaults
    this.brandPath = environment.brandURL;
  }

  // -----------------------------------------------------------------------------------------------------
  // @ Public methods
  // -----------------------------------------------------------------------------------------------------

  /**
   * Uplode files
   */
  uploadFile(filesToUpload: File[]): any
  {

    const formData = new FormData();

    Array.from(filesToUpload).map((file, index) => {
      return formData.append('file' + index, file, file.name);
    });
    return this._httpClient.post(this.brandPath+'/upload', formData, { reportProgress: true, observe: 'events' })
      .subscribe(
        {
          next: (event) => {
            // if (event.type === HttpEventType.UploadProgress)
            //   this.progress = Math.round(100 * event.loaded / event.total);
            // else if (event.type === HttpEventType.Response) {
            //   this.message = 'Upload success.';
            //   this.onUploadFinished.emit(event.body);
            // }
          },
          error: (err: HttpErrorResponse) => console.log(err)
        });
  }
 

   

    
  }

