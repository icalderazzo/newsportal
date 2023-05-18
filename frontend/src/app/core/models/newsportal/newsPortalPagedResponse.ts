import { NewsPortalResponse } from "./newsPortalResponse";

export interface NewsPortalPagedResponse<T> extends NewsPortalResponse<T>{
    pageNumber: number,
    pageSize: number,
    totalPages: number,
    totalRecords: number,
    previousPage: string | undefined,
    nextPage: string | undefined
}