import { API_URL } from '$lib/consts'

/** @type {import('./$types').PageLoad} */
export async function load({ fetch, params, parent }) {

    try {
        const response = await fetch(`${API_URL}/news/${params.id}`)
        const news = await response.json()
        const { user } = await parent()

        return { news, user }

    } catch (e) {
        console.log(e)
    }
}