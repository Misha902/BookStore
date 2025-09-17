import { RestService, Rest } from '@abp/ng.core';
import type { ListResultDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { AuthorLookupDto } from '../authors/models';
import type { BookDto, CreateBookDto, GetBookListDto, UpdateBookDto } from '../books/models';

@Injectable({
  providedIn: 'root',
})
export class BookService {
  apiName = 'BookStore';
  

  create = (input: CreateBookDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, BookDto>({
      method: 'POST',
      url: '/api/app/mycontroller/book',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/mycontroller/book/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, BookDto>({
      method: 'GET',
      url: `/api/app/mycontroller/book/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getAuthorLookup = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<AuthorLookupDto>>({
      method: 'GET',
      url: '/api/app/mycontroller/book/author-lookup',
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetBookListDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<BookDto>>({
      method: 'GET',
      url: '/api/app/mycontroller/book',
      params: { filter: input.filter, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getListNoTrack = (input: GetBookListDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<BookDto>>({
      method: 'GET',
      url: '/api/app/mycontroller/book/booksntr',
      params: { filter: input.filter, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: UpdateBookDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'PUT',
      url: `/api/app/mycontroller/book/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
