export class PaginatedComponentBase<T> {
    currentPage: number;
    totalItems: number;
    itemsPerPage: number;

    initializePagination(data): T[] {
        this.totalItems = data.pagination.totalItems;
        this.itemsPerPage = data.pagination.itemsPerPage;
        this.currentPage = data.pagination.currentPage;
        return data.result;
    }
}
