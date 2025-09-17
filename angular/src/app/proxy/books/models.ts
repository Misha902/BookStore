import type { AuditedEntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { BookType } from './book-type.enum';

export interface BookDto extends AuditedEntityDto<string> {
  name?: string;
  type?: BookType;
  publishDate?: string;
  price: number;
  authorId?: string;
  authorName?: string;
  quantity: number;
}

export interface CreateBookDto {
  name: string;
  type: BookType;
  publishDate: string;
  price: number;
  authorId?: string;
  quantity: number;
}

export interface GetBookListDto extends PagedAndSortedResultRequestDto {
  filter?: string;
}

export interface UpdateBookDto {
  name: string;
  type: BookType;
  publishDate: string;
  price: number;
  authorId?: string;
  quantity: number;
}
