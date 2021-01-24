interface PagedResult<T> {
  pageSize?: number;
  pageIndex?: number;
  totalPages?: number;
  count?: number;
  totalCount?: number;
  data: Array<T>;
}

interface PagedQuery {
  pageSize: number;
  pageIndex: number;
}

interface SelectListItem {
  value: any;
  text: string;
  disabled?: boolean;
}

declare var tinymce: any;
