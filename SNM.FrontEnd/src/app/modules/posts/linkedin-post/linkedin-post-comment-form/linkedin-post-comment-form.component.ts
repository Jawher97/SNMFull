import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'linkedin-post-comment-form',
  templateUrl: './linkedin-post-comment-form.component.html',
  styleUrls: ['./linkedin-post-comment-form.component.scss']
})
export class LinkedinPostCommentFormComponent {
  @Input() submitLabel!: string;
  @Input() hasCancelButton: boolean = false;
  @Input() initialText: string = '';

  form!: FormGroup;

  @Output() handleSubmit = new EventEmitter<string>();
  @Output() handleCancel = new EventEmitter<void>();

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      title: [this.initialText, Validators.required],
    });
  }


 

  // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------
    onSubmit(): void{
      this.handleSubmit.emit(this.form.value.title);
      this.form.reset();
    }
 /**
     * Discard the message
     */
 discard(): void
 {

  this.form.reset();
  this.handleSubmit.emit(this.form.value.title);
 }
 /**
     * Discard the message
     */
 send(): void
 {

 }
}
