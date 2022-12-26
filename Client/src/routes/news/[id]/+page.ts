/** @type {import('./$types').PageLoad} */
export async function load({ fetch, params }) {
    let news = null
    try {
        const response = await fetch(`https://localhost:7220/api/news/${params.id}`)
        news = await response.json()
    } catch (e) {
        console.log(e)
    }

    return { news }
}