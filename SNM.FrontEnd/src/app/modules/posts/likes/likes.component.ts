import {Component, Inject} from '@angular/core';
import {MatDialog, MAT_DIALOG_DATA, MatDialogRef, MatDialogModule} from '@angular/material/dialog';

@Component({
  selector: 'app-likes',
  templateUrl: './likes.component.html',
  styleUrls: ['./likes.component.scss']
})
export class LikesComponent {
  constructor(
    public dialogRef: MatDialogRef<LikesComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    ) {}
  ngOnInit(): void {
   
    console.log(JSON.stringify(this.data)+'pppppost')
    console.log(JSON.stringify(this.data.count)+'pppppost')
    console.log(JSON.stringify(this.data.reactions)+'pppppost')
    if(this.data.count>this.data.reactions){
      this.data.reactions.push({name:this.data.name,picture:this.data.photo,reactionType:2})
      console.log(JSON.stringify(this.data.reactions)+'pppppost')
    }
  }
  onClose(): void {
    this.dialogRef.close();
  }
 
  
}
export interface DialogData {
  reactions:any
  name: string;
  photo:any;
  count:any

}