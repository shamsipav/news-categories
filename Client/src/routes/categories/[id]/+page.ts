import { API_URL } from '$lib/consts'

/** @type {import('./$types').PageLoad} */
export async function load({ fetch, params, parent }) {
    try {
        const response = await fetch(`${API_URL}/category/${params.id}`)
        const category = await response.json()
        const { user } = await parent()

        return { category, user }
    } catch (e) {
        console.log(e)
    }
}