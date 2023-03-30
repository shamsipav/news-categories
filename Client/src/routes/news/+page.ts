import { API_URL } from '$lib/consts'

export async function load({ fetch, parent }) {
    try {
        const response = await fetch(`${API_URL}/news`)
        const news = await response.json()
        const { user } = await parent()

        return { news, user }
    } catch (e) {
        console.log(e)
    }
}