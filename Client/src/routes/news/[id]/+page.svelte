<script lang="ts">
    import dayjs from 'dayjs'
    import { slide } from 'svelte/transition'
    import { Modal, Form } from '$components'
    import type { INews, ICategory, ModalComponent, IUser } from '../../../lib/types'
    import { onMount } from 'svelte'
    import { API_URL } from '$lib/consts'

    export let data: any

    let user: IUser = data.user

    let news: INews = data.news
    let newsCategoriesIds = news?.categories.map(c => c.id)

    let modal: ModalComponent = null
    let modalDelete: ModalComponent = null

    const handleSuccess = (event: CustomEvent<{ message: string, news: INews }>) => {
        news = event.detail.news
        newsCategoriesIds = news?.categories.map(c => c.id)
        setTimeout(modal.close, 500)
    }

    let categories: ICategory[]
    const getCategories = async () => {
        try {
            const response = await fetch(`${API_URL}/category`)
            categories = await response.json()
        } catch (e) {
            console.log(e)
        }
    }

    let categoriesFormVisible = false
    const showNewCategory = (event: CustomEvent<{ message: string, category: ICategory }>) => {
        const newCategory = event.detail.category
        categories = [ ...categories, newCategory ]
        setTimeout(() => categoriesFormVisible = false, 500)
    }

    onMount(async () => {
        getCategories()
    })
</script>

<div class="container">
    {#if news}
        <div class="d-inline-flex align-items-center">
            <h2 class="me-3">{news.title}</h2>
            {#if user}
                <button
                    type="button"
                    class="btn btn-primary me-2 btn-sm"
                    on:click={modal.open}>Редактировать</button
                >
                <button
                    type="button"
                    class="btn btn-danger btn-sm"
                    on:click={modalDelete.open}>Удалить</button
                >
            {/if}
        </div>
        <p class="mb-2">{dayjs(news.createTime).format('D MMMM YYYY')}</p>
        {#each news.categories as category}
            <span class="badge bg-secondary me-2">{category.name}</span>
        {/each}
        <p class="mt-4">{news.text}</p>
    {:else}
        <p>Загрузка новости...</p>
    {/if}
</div>
<Modal bind:this={modal} hasFooter={false} title="Редактирование новости">
    <Form method="PUT" action="{API_URL}/news/{news.id}" reset={false} on:success={handleSuccess} extended>
        <div class="mb-3">
            <label for="title" class="form-label">Заголовок</label>
            <input type="text" name="title" class="form-control" id="title" value={news.title} />
        </div>
        <div class="mb-3">
            <label for="text" class="form-label">Текст новости</label>
            <textarea name="text" class="form-control" id="text" cols="30" rows="5">{news.text}</textarea>
        </div>
        <div class="mb-3">
            <label for="createTime" class="form-label">Дата публикации</label>
            <input type="date" name="createTime" class="form-control" id="createTime" value={dayjs(news.createTime).format('YYYY-MM-DD')} />
        </div>
        <div class="mb-3">
            <div>
                <label for="categories" class="form-label me-2">Категории</label>
                <button type="button" class="btn btn-success btn-sm" on:click={() => categoriesFormVisible = !categoriesFormVisible}>{categoriesFormVisible ? 'Свернуть' : 'Новая'}</button>
            </div>
            {#if categoriesFormVisible}
                <div transition:slide|local={{ duration: 200 }} class="mb-3">
                    <Form method="POST" action="{API_URL}/category" reset={true} on:success={showNewCategory}>
                        <div class="mb-3">
                            <label for="name" class="form-label">Название</label>
                            <input type="text" name="name" class="form-control" required />
                        </div>
                        <button class="btn btn-primary btn-sm">Добавить</button>
                    </Form>
                </div>
            {/if}
            {#if categories}
                <div class="categories">
                    {#each categories as category, i}
                      <div class="form-check">
                            <input type="text" name="category_name{i}" value="{category.name}" hidden>
                            {#if newsCategoriesIds.includes(category.id)}
                                <input class="form-check-input" type="checkbox" name="category_id{i}" value="{category.id}" id="checkChecked{category.id}" checked>
                                <label class="form-check-label" for="checkChecked{category.id}">
                                    {category.name}
                                </label>
                            {:else}
                                <input class="form-check-input" type="checkbox" name="category_id{i}" value="{category.id}" id="checkChecked{category.id}">
                                <label class="form-check-label" for="checkChecked{category.id}">
                                    {category.name}
                                </label>
                            {/if}
                      </div>
                    {/each}
                </div>
            {/if}
        </div>
        <button class="btn btn-primary mb-3">Сохранить</button>
    </Form>
</Modal>

<Modal bind:this={ modalDelete } hasFooter={ false } title="Удаление новости">
    <Form method="DELETE" action="{API_URL}/news/{news.id}" redirectTo="/news">
        <p>Вы действительно хотите удалить новость "{news.title}" от {dayjs(news.createTime).format('D MMMM YYYY')}?</p>
        <button class="btn btn-danger mb-3 btn-sm">Удалить</button>
        <button type="button" class="btn btn-primary mb-3 btn-sm" on:click={modalDelete.close}>Отмена</button>
    </Form>
</Modal>
