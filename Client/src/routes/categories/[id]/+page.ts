/** @type {import('./$types').PageLoad} */
export async function load({ fetch, params }) {
    let category = null
    try {
        const response = await fetch(`https://localhost:7220/api/category/${params.id}`)
        category = await response.json()
    } catch (e) {
        console.log(e)
    }

    return { category }
}