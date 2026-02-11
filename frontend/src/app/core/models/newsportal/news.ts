export interface News {
    id: number,
    title: string,
    text: string | undefined,
    url: string | undefined,
    isBookmarked: boolean,
    time: Date
}