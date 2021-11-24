using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using StarterProject.Context.Contexts;

namespace StarterProject.Context.Base
{
    public static class ContextExtensions
    {
        public static void UpdateChildCollection<Tparent, Tchild>(this AppDbContext Context, Tparent dbItem, Tparent newItem, Func<Tparent, IEnumerable<Tchild>> selector)
            where Tchild : class, IEntity
        {
            var dbItems = selector(dbItem).ToList();
            var newItems = selector(newItem).ToList();

            var original = dbItems?.ToDictionary(c => c.Id) ?? new Dictionary<int, Tchild>();

            var updated = new List<KeyValuePair<int, Tchild>>();

            foreach (var item in newItems.Where(c => c.Id > 0))
            {
                updated.Add(new KeyValuePair<int, Tchild>(item.Id, item));
            }

            var added = new List<KeyValuePair<int, Tchild>>();

            foreach (var item in newItems.Where(c => c.Id == 0))
            {
                added.Add(new KeyValuePair<int, Tchild>(item.Id, item));
            }

            var toRemove = original.Where(i => !updated.Select(c => c.Key).Contains(i.Key)).ToArray();
            var removed = toRemove.Select(i => Context.Entry(i.Value).State = EntityState.Deleted).ToArray();

            var toUpdate = original.Where(i => updated.Select(c => c.Key).Contains(i.Key)).ToList();
            toUpdate.ForEach(i => Context.Entry(i.Value).CurrentValues.SetValues(updated.Single(c => c.Key.Equals(i.Key)).Value));
            //toAdd.ForEach(i => Context.Set<Tchild>().Add(i.Value));

            added.ForEach(i => (selector(dbItem) as ICollection<Tchild>).Add(i.Value));
        }

        public static void UpdateChildCollectionNtoN<Tparent, Tchild>(this AppDbContext Context, Tparent dbItem, Tparent newItem, Func<Tparent, IEnumerable<Tchild>> selector, IEqualityComparer<Tchild> equalityComparer)
            where Tchild : class
        {
            var dbItems = selector(dbItem).ToList();
            var newItems = selector(newItem).ToList();

            var added = newItems.Except(dbItems, equalityComparer);
            var removed = dbItems.Except(newItems, equalityComparer);

            foreach (var item in added)
            {
                (selector(dbItem) as ICollection<Tchild>).Add(item);
            }

            foreach (var item in removed)
            {
                Context.Entry(item).State = EntityState.Deleted;
            }
        }

        public static List<string> GetChanges(this EntityEntry entityEntry)
        {
            List<string> lista = new List<string>();
            foreach (var entry in entityEntry.Properties.Where(c => c.IsModified))
            {
                if (entry.OriginalValue != entry.CurrentValue)
                {
                    lista.Add($"Campo '{entry.Metadata.Name}' alterado de '{entry.OriginalValue}' para '{entry.CurrentValue}'");
                }
            }
            return lista;
        }

        public static void AddOrUpdate<T>(this AppDbContext Context, T dbItem)
            where T : class, IEntity
        {
            if (dbItem.Id == 0)
            {
                Context.Add(dbItem);
            }
            else
            {
                var entityEntry = Context.Entry(dbItem);

                if (entityEntry.State != EntityState.Modified && entityEntry.State != EntityState.Unchanged)
                {
                    throw new InvalidOperationException($"AddOrUpdate: não é possível atualizar um item com status {entityEntry.State}.");
                }

                Context.Update(dbItem);
            }
        }
    }
}
