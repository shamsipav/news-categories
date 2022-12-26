<script lang="ts">
    import dayjs from 'dayjs'
    import { Modal, Form } from '$components'
    import { slide } from 'svelte/transition'
    import type { INews, ICategory, ModalComponent } from '../../lib/types'

    export let data: any
    
    let news: INews[] = data.news
    let modal: ModalComponent = null

    const showNewNews = (event: CustomEvent<{ message: string, news: INews }>) => {
        const newNews = event.detail.news
        news = [ ...news, newNews ]
        setTimeout(modal.close, 500)
    }

    let categories: ICategory[]
    const getCategories = async () => {
        try {
            const response = await fetch('https://localhost:7220/api/category')
            categories = await response.json()
        } catch (e) {
            console.log(e)
        }
    }

    let categoriesFormVisible = false;
    const showNewCategory = (event: CustomEvent<{ message: string, category: ICategory }>) => {
        const newCategory = event.detail.category
        categories = [ ...categories, newCategory ]
        setTimeout(() => categoriesFormVisible = false, 500)       
    }

    getCategories()
</script>

<div class="container">
    <h2 class="mb-4">Новости</h2>
    <button type="button" class="btn btn-primary mb-3" on:click={modal.open}>Добавить новость</button>
    {#if news}
        <div class="news-grid">
            {#each news as newsItem}
                <div class="card">
                    <div class="card-body">
                        <h5 class="card-title">{newsItem.title}</h5>
                        <p class="mb-2">{dayjs(newsItem.createTime).format('D MMMM YYYY')}</p>
                        {#each newsItem.categories as category}
                            <span class="badge bg-secondary me-2">{category.name}</span>
                        {/each}
                        <p class="card-text mt-2">{newsItem.text}</p>
                        <a href="/news/{newsItem.id}" class="btn btn-primary btn-sm">Читать полностью</a>
                    </div>
                </div>
            {/each}
        </div>
    {:else}
        <p>Загрузка новостей...</p>
    {/if}
</div>
<Modal bind:this={ modal } hasFooter={ false } title="Добавление новости">
    <Form method="POST" action="https://localhost:7220/api/News" content="application/json" reset={ false } on:success={ showNewNews }>
        <div class="mb-3">
            <label for="title" class="form-label">Заголовок</label>
            <input type="text" name="title" class="form-control" id="title" required />
        </div>
        <div class="mb-3">
            <label for="text" class="form-label">Текст новости</label>
            <textarea type="text" name="text" class="form-control" id="text" cols="30" rows="5" required />
        </div>
        <div class="mb-3">
            <div>
                <label for="categories" class="form-label me-2">Категории</label>
                <button type="button" class="btn btn-success btn-sm" on:click={ () => categoriesFormVisible = !categoriesFormVisible }>{categoriesFormVisible ? 'Свернуть' : 'Новая'}</button>
            </div>
            {#if categoriesFormVisible}
                <div transition:slide|local={{ duration: 200 }} class="mb-3">
                    <Form method="POST" action="https://localhost:7220/api/Category" content="application/json" reset={ true } on:success={ showNewCategory }>
                        <div class="mb-3">
                            <label for="name" class="form-label">Название</label>
                            <input type="text" name="name" class="form-control" required />
                        </div>
                        <input type="text" name="news" hidden />
                        <button class="btn btn-primary btn-sm">Добавить</button>
                    </Form>
                </div>
            {/if}
            {#if categories}
                <div class="categories">
                    {#each categories as category}
                      <div class="form-check">
                          <input type="text" name="names" value="{category.name}" hidden>
                          <!-- <input class="form-check-input" type="checkbox" name="categories" value="{category.id}" id="checkChecked{category.id}"> -->
                          <input class="form-check-input" type="checkbox" name="categories" value="{category.id}" id="checkChecked{category.id}">
                          <label class="form-check-label" for="checkChecked{category.id}">
                              {category.name}
                          </label>
                      </div>
                    {/each}
                </div>
            {/if}
        </div>
        <button class="btn btn-primary mb-3 btn-sm">Добавить</button>
    </Form>
</Modal>

