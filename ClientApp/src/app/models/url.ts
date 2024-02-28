export interface UrlModel {
  id: number;
  url: string;
  shortUrl: string;
  createdBy: string;
  createdDate: Date;
}

export interface ApiResponse<T> {
  result: null | T;
  value: T;
}
