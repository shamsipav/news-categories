import { API_URL } from '$lib/consts'

/** @type {import('./$types').PageLoad} */
export async function load({ fetch, parent }) {
    try {
        const response = await fetch(`${API_URL}/category`)
        const categories = await response.json()
        const { user } = await parent()

        return { categories, user}
    } catch (e) {
        console.log(e)
    }
}