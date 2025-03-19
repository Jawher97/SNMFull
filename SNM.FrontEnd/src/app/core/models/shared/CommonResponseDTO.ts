export class ResponseModel<T> {

    isSuccess: boolean;
    responseData: T;
    code: number;
    message: string;
   constructor (T:ResponseModel<T>)
    {
      this.responseData=T.responseData;
      this.isSuccess=T.isSuccess;
      this.code=T.code;
      this.message=T.message;
    }
  }