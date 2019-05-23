using SeniorProject.Api.Models;
using SeniorProject.Api.Models.Entities;
using SeniorProject.Api.Models.FromApp;
using SeniorProject.Api.Models.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SeniorProject.Api.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<PagedResults<T>> GetEntitiesAsync(CancellationToken ct);
        Task<T> GetEntityAsync(int id, CancellationToken ct);
        Task<bool> AddEntityAsync(T entity, CancellationToken ct);

        Task<List<T>> GetAllEntities(CancellationToken ct);
        Task<T> UpdateEntity(T entity, CancellationToken ct);
        Task<bool> DeleteEntity(int id, CancellationToken ct);
    }

    public interface IShoppingUserRepository : IRepository<ShoppingUserEntity>
    {
        Task<Tuple<bool, string>> AddEntityAsync(APIShoppingUser entity);
        Task<ShoppingUserEntity> GetEntityAsync(string email, CancellationToken ct);
        Task<ShoppingUserEntity> GetEntityAsync(Guid id, CancellationToken ct);
    }

    public interface IStoreUserRepository : IRepository<StoreUserEntity>
    {
        Task<Tuple<bool, string>> AddEntityAsync(APIStoreUser entity, CancellationToken ct);
        Task<StoreUserEntity> GetEntityAsync(string email, CancellationToken ct);
        Task<StoreUserEntity> GetEntityAsync(Guid id, CancellationToken ct);
    }

    public interface IShoppingListRepository : IRepository<ShoppingListEntity>
    {
        Task<List<ShoppingListEntity>> GetAllShoppingListsForUser(Guid userId, CancellationToken ct);
        Task<List<ShoppingListEntity>> GetAllShoppingListsForUserForACertainStore(Guid userId, int storeId, CancellationToken ct);
        Task<Tuple<bool, string>> AddShoppingList(ShoppingListEntity entity, CancellationToken ct);
    }


    public interface IItemStoreLinkRepository : IRepository<ItemStoreLinkEntity>
    {
        Task<PagedResults<ItemStoreLinkEntity>> GetEntitiesAsync(int storeId, CancellationToken ct);
        Task<List<ItemStoreLinkEntity>> GetAllEntities(int storeId, CancellationToken ct);

        Task<ItemStoreLinkEntity> GetEntityAsync(int itemId, int storeId, CancellationToken ct);
    }

    public interface IItemEntityRepository : IRepository<ItemEntity>
    {
        Task<ItemEntity> GetEntityBySpoonIdAsync(int spoonId, CancellationToken ct);
    }

    public interface IItemShoppingListLinkRepository : IRepository<ItemShoppingListLinkEntity>
    {
        Task<List<ItemShoppingListLinkEntity>> GetAllByShoppingListId(int shoppingListId, CancellationToken ct);

        Task<ItemShoppingListLinkEntity> GetShoppingItem(int itemShoppingListLinkId, CancellationToken ct);

        Task<ItemShoppingListLinkEntity> AddNewLink(ItemShoppingListLinkEntity entity, CancellationToken ct);
    }

    public interface ILowerDepartmentRepository : IRepository<LowerDepartmentEntity>
    {
        List<LowerDepartmentEntity> GetAllEntities(int depertmentId, CancellationToken ct);
    }

    public interface IAisleRepository : IRepository<AisleEntity>
    {
        List<AisleEntity> GetAllEntities(int lowerDepartmentId, CancellationToken ct);
    }

    public interface ISectionRepository : IRepository<SectionEntity>
    {
        List<SectionEntity> GetAllEntities(int aisleId, CancellationToken ct);
    }

    public interface IShelfRepository : IRepository<ShelfEntity>
    {
        List<ShelfEntity> GetAllEntities(int sectionId, CancellationToken ct);
    }

    public interface IShelfSlotsRepository : IRepository<ShelfSlotEntity>
    {
        List<ShelfSlotEntity> GetAllEntities(int shelfId, CancellationToken ct);
    }

}
