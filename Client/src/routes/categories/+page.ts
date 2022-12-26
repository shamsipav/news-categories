/** @type {import('./$types').PageLoad} */
export async function load({ fetch }) {
    let categories = null
    try {
        const response = await fetch('https://localhost:7220/api/Category')
        categories = await response.json()
    } catch (e) {
        console.log(e)
    }

    return { categories }
}