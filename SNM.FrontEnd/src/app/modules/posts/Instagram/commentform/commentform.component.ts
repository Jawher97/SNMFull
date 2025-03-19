import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'commentForm',
  templateUrl: './commentform.component.html',
  styleUrls: ['./commentform.component.scss']
})
export class CommentformComponent {

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


  onSubmit(): void{
    this.handleSubmit.emit(this.form.value.title);
    this.form.reset();
  }
}
