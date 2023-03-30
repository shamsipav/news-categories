export interface IUser {
    id: number;
    firstName: string;
    lastName: string;
    email: string;
    lastLoginIp: string;
    lastLoginDate: string;
}
export interface ICategory {
    id: number;
    name: string;
    news: INews[];
}

export interface INews {
    id: number;
    title: string;
    text: string;
    createTime: string;
    categories: ICategory[];
}


export interface ModalComponent {
    open: () => void,
    close: () => void,
    toggle: () => void
}

export type RESTMethod = 'GET' | 'POST' | 'PUT' | 'DELETE' | 'PATCH'
