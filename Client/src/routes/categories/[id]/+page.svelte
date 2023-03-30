<script lang="ts">
    import dayjs from 'dayjs'
    import { Modal, Form } from '$components'
    import type { ICategory, ModalComponent, IUser } from '../../../lib/types'
    import { API_URL } from '$lib/consts'

    export let data: any

    let user: IUser = data.user

    let category: ICategory = data.category

    let modal: ModalComponent = null
    let modalDelete: ModalComponent = null

    const handleSuccess = (event: CustomEvent<{ message: string, category: ICategory }>) => {
        category = event.detail.category
        setTimeout(modal.close, 500)
    }
</script>

<div class="container">
    {#if category}
        <div class="d-inline-flex align-items-center mb-4">
            <h2 class="me-3">{category.name}</h2>
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
        <h4 class="mb-4">Новости категории</h4>
        {#if category.news.length > 0}
            <div class="news-grid">
                {#each category.news as newsItem}
                    <div class="card">
                        <div class="card-body">
                            <h5 class="card-title">{newsItem.title}</h5>
                            <p class="mb-2">{dayjs(newsItem.createTime).format('D MMMM YYYY')}</p>
                            <p class="card-text mt-2">{newsItem.text}</p>
                            <a href="/news/{newsItem.id}" class="btn btn-primary btn-sm">Читать полностью</a>
                        </div>
                    </div>
                {/each}
            </div>
        {:else}
            <p>У данной категории еще нет новостей</p>
        {/if}
    {:else}
        <p>Загрузка категории...</p>
    {/if}
</div>
<Modal bind:this={ modal } hasFooter={ false } title="Редактирование категории">
    <Form method="PUT" action="{API_URL}/category/{category.id}" reset={ false } on:success={ handleSuccess }>
        <div class="mb-3">
            <label for="name" class="form-label">Название</label>
            <input type="text" name="name" class="form-control" id="name" value={category.name} />
        </div>
        <button class="btn btn-primary mb-3">Сохранить</button>
    </Form>
</Modal>

<Modal bind:this={ modalDelete } hasFooter={ false } title="Удаление категории">
    <Form method="DELETE" action="{API_URL}/category/{category.id}" redirectTo="/categories">
        <p>Вы действительно хотите удалить категорию "{category.name}"?</p>
        <button class="btn btn-danger mb-3 btn-sm">Удалить</button>
        <button type="button" class="btn btn-primary mb-3 btn-sm" on:click={modalDelete.close}>Отмена</button>
    </Form>
</Modal>

