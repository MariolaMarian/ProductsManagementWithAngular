import { IPagination } from '../interfaces/pagination.interface';

export class PaginatedResult<T> {
    result: T;
    pagination: IPagination;
}
