using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using NSubstitute;
using NUnit.Framework;
using u2.Core.Contract;
using u2.Test;

namespace u2.Core.Test
{
    [TestFixture]
    public class MapTaskTest
    {
        [Test]
        public void Constructor_success()
        {
            var map = new MapTask<TestItem>();

            Assert.That(map.Alias, Is.EqualTo("testitem"));
            Assert.That(map.EntityType, Is.EqualTo(typeof(TestItem)));
        }

        [Test]
        public void All_success()
        {
            var map = new MapTask<TestItem>().All();

            Assert.That(map.Maps.Count, Is.EqualTo(5));
        }

        [Test]
        public void Create_new_success()
        {
            var map = new MapTask<TestItem>();
            var result = map.Create();

            Assert.That(result is TestItem);
        }

        [Test]
        public void Create_with_instance()
        {
            var map = new MapTask<TestItem>();
            var instance = new TestItem {ItemId = 100};
            var result = map.Create(instance) as TestItem;

            Assert.That(result, Is.EqualTo(instance));
            Assert.That(result?.ItemId, Is.EqualTo(100));
        }


        [Test]
        public void Create_return_null()
        {
            var map = new MapTask<TestItem>();
            var instance = new TestEntity();
            var result = map.Create(instance);

            Assert.That(result is TestItem);
        }

        [Test]
        public void AliasTo_success()
        {
            var map = new MapTask<TestItem>()
                .AliasTo("ABC");
            Assert.That(map.Alias, Is.EqualTo("abc"));
        }

        [Test]
        public void AliasTo_null()
        {
            var map = new MapTask<TestItem>()
                .AliasTo(null);
            Assert.That(map.Alias, Is.EqualTo("testitem"));
        }

        [Test]
        public void Map_property()
        {
            var map = new MapTask<TestItem>()
                .Map(x => x.ItemId);

            Assert.That(map.Maps.Count, Is.EqualTo(1));
            var mapItem = map.Maps.First();
            Assert.That(mapItem.Alias, Is.EqualTo("itemid"));
            Assert.That(mapItem.Setter, Is.Not.Null);
            var item = new TestItem();
            mapItem.Setter.Set(item, 8);
            Assert.That(item.ItemId, Is.EqualTo(8));
        }

        [Test]
        public void Map_null()
        {
            var map = new MapTask<TestItem>()
                .Map<int>(null);

            Assert.That(map.Maps.Count, Is.EqualTo(0));
        }

        [Test]
        public void Map_alias()
        {
            var map = new MapTask<TestItem>()
                .Map(x => x.ItemId, "iD");

            var mapItem = map.Maps.First();
            Assert.That(mapItem.Alias, Is.EqualTo("id"));
        }

        [Test]
        public void Map_func()
        {
            var map = new MapTask<TestItem>()
                .Map(x => x.ItemId, mapFunc: x => int.Parse(x) * 2);

            var mapItem = map.Maps.First();
            var item = new TestItem();
            var value = mapItem.Converter("3");
            mapItem.Setter.Set(item, value);
            Assert.That(item.ItemId, Is.EqualTo(6));
        }

        [Test]
        public void Map_default()
        {
            var map = new MapTask<TestItem>()
                .Map(x => x.ItemId, defaultVal: 1);

            var mapItem = map.Maps.First();
            Assert.That(mapItem.Default, Is.EqualTo(1));
        }

        [Test]
        public void Act_1()
        {
            var map = new MapTask<TestAction>()
                .Act<int>((x, v) => x.Sum = v + 1, "value1");

            var action = map.GroupActions.First();
            var field = map.CmsFields.First();
            var item = new TestAction();

            Assert.That(map.CmsFields.Count, Is.EqualTo(1));
            Assert.That(map.Maps.Count, Is.EqualTo(0));
            Assert.That(field.Key, Is.EqualTo("value1"));
            Assert.That(field.Value, Is.EqualTo(typeof(int)));
            Assert.That(action.Aliases.First(), Is.EqualTo("value1"));
            action.Action(item, new object[] {2});
            Assert.That(item.Sum, Is.EqualTo(3));
        }

        [Test]
        public void Act_2()
        {
            var map = new MapTask<TestAction>()
                .Act<int, int>((x, v1, v2) => x.Sum = v1 + v2, "value1", "value2");

            var action = map.GroupActions.First();
            var first = map.CmsFields.First();
            var second = map.CmsFields.Skip(1).Take(1).First();
            var item = new TestAction();

            Assert.That(map.CmsFields.Count, Is.EqualTo(2));
            Assert.That(first.Key, Is.EqualTo("value1"));
            Assert.That(first.Value, Is.EqualTo(typeof(int)));
            Assert.That(second.Key, Is.EqualTo("value2"));
            Assert.That(second.Value, Is.EqualTo(typeof(int)));
            Assert.That(action.Aliases.First(), Is.EqualTo("value1"));
            Assert.That(action.Aliases.Skip(1).Take(1).First(), Is.EqualTo("value2"));
            action.Action(item, new object[] {1, 2});
            Assert.That(item.Sum, Is.EqualTo(3));
        }

        [Test]
        public void Act_3()
        {
            var map = new MapTask<TestAction>()
                .Act<int, int, int>((x, v1, v2, v3) => x.Sum = v1 + v2 + v3, "value1", "value2", "value3");

            var action = map.GroupActions.First();
            var first = map.CmsFields.First();
            var second = map.CmsFields.Skip(1).Take(1).First();
            var third = map.CmsFields.Skip(2).Take(1).First();
            var item = new TestAction();

            Assert.That(map.CmsFields.Count, Is.EqualTo(3));
            Assert.That(first.Key, Is.EqualTo("value1"));
            Assert.That(first.Value, Is.EqualTo(typeof(int)));
            Assert.That(second.Key, Is.EqualTo("value2"));
            Assert.That(second.Value, Is.EqualTo(typeof(int)));
            Assert.That(third.Key, Is.EqualTo("value3"));
            Assert.That(third.Value, Is.EqualTo(typeof(int)));
            Assert.That(action.Aliases.First(), Is.EqualTo("value1"));
            Assert.That(action.Aliases.Skip(1).Take(1).First(), Is.EqualTo("value2"));
            Assert.That(action.Aliases.Skip(2).Take(1).First(), Is.EqualTo("value3"));
            action.Action(item, new object[] {1, 2, 3});
            Assert.That(item.Sum, Is.EqualTo(6));
        }

        [Test]
        public void Act_4()
        {
            var map = new MapTask<TestAction>()
                .Act<int, int, int, int>((x, v1, v2, v3, v4) => x.Sum = v1 + v2 + v3 + v4, "value1", "value2", "value3",
                    "value4");

            var action = map.GroupActions.First();
            var first = map.CmsFields.First();
            var second = map.CmsFields.Skip(1).Take(1).First();
            var third = map.CmsFields.Skip(2).Take(1).First();
            var forth = map.CmsFields.Skip(3).Take(1).First();
            var item = new TestAction();

            Assert.That(map.CmsFields.Count, Is.EqualTo(4));
            Assert.That(first.Key, Is.EqualTo("value1"));
            Assert.That(first.Value, Is.EqualTo(typeof(int)));
            Assert.That(second.Key, Is.EqualTo("value2"));
            Assert.That(second.Value, Is.EqualTo(typeof(int)));
            Assert.That(third.Key, Is.EqualTo("value3"));
            Assert.That(third.Value, Is.EqualTo(typeof(int)));
            Assert.That(forth.Key, Is.EqualTo("value4"));
            Assert.That(forth.Value, Is.EqualTo(typeof(int)));
            Assert.That(action.Aliases.First(), Is.EqualTo("value1"));
            Assert.That(action.Aliases.Skip(1).Take(1).First(), Is.EqualTo("value2"));
            Assert.That(action.Aliases.Skip(2).Take(1).First(), Is.EqualTo("value3"));
            Assert.That(action.Aliases.Skip(3).Take(1).First(), Is.EqualTo("value4"));
            action.Action(item, new object[] {1, 2, 3, 4});
            Assert.That(item.Sum, Is.EqualTo(10));
        }

        [Test]
        public void Act_5()
        {
            var map = new MapTask<TestAction>()
                .Act<int, int, int, int, int>((x, v1, v2, v3, v4, v5) => x.Sum = v1 + v2 + v3 + v4 + v5, "value1",
                    "value2", "value3", "value4", "value5");

            var action = map.GroupActions.First();
            var first = map.CmsFields.First();
            var second = map.CmsFields.Skip(1).Take(1).First();
            var third = map.CmsFields.Skip(2).Take(1).First();
            var forth = map.CmsFields.Skip(3).Take(1).First();
            var fifth = map.CmsFields.Skip(4).Take(1).First();
            var item = new TestAction();

            Assert.That(map.CmsFields.Count, Is.EqualTo(5));
            Assert.That(first.Key, Is.EqualTo("value1"));
            Assert.That(first.Value, Is.EqualTo(typeof(int)));
            Assert.That(second.Key, Is.EqualTo("value2"));
            Assert.That(second.Value, Is.EqualTo(typeof(int)));
            Assert.That(third.Key, Is.EqualTo("value3"));
            Assert.That(third.Value, Is.EqualTo(typeof(int)));
            Assert.That(forth.Key, Is.EqualTo("value4"));
            Assert.That(forth.Value, Is.EqualTo(typeof(int)));
            Assert.That(fifth.Key, Is.EqualTo("value5"));
            Assert.That(fifth.Value, Is.EqualTo(typeof(int)));
            Assert.That(action.Aliases.First(), Is.EqualTo("value1"));
            Assert.That(action.Aliases.Skip(1).Take(1).First(), Is.EqualTo("value2"));
            Assert.That(action.Aliases.Skip(2).Take(1).First(), Is.EqualTo("value3"));
            Assert.That(action.Aliases.Skip(3).Take(1).First(), Is.EqualTo("value4"));
            Assert.That(action.Aliases.Skip(4).Take(1).First(), Is.EqualTo("value5"));
            action.Action(item, new object[] {1, 2, 3, 4, 5});
            Assert.That(item.Sum, Is.EqualTo(15));
        }

        [Test]
        public void Act_6()
        {
            var map = new MapTask<TestAction>()
                .Act<int, int, int, int, int, int>((x, v1, v2, v3, v4, v5, v6) => x.Sum = v1 + v2 + v3 + v4 + v5 + v6,
                    "value1", "value2", "value3", "value4", "value5", "value6");

            var action = map.GroupActions.First();
            var first = map.CmsFields.First();
            var second = map.CmsFields.Skip(1).Take(1).First();
            var third = map.CmsFields.Skip(2).Take(1).First();
            var forth = map.CmsFields.Skip(3).Take(1).First();
            var fifth = map.CmsFields.Skip(4).Take(1).First();
            var sixth = map.CmsFields.Skip(5).Take(1).First();
            var item = new TestAction();

            Assert.That(map.CmsFields.Count, Is.EqualTo(6));
            Assert.That(first.Key, Is.EqualTo("value1"));
            Assert.That(first.Value, Is.EqualTo(typeof(int)));
            Assert.That(second.Key, Is.EqualTo("value2"));
            Assert.That(second.Value, Is.EqualTo(typeof(int)));
            Assert.That(third.Key, Is.EqualTo("value3"));
            Assert.That(third.Value, Is.EqualTo(typeof(int)));
            Assert.That(forth.Key, Is.EqualTo("value4"));
            Assert.That(forth.Value, Is.EqualTo(typeof(int)));
            Assert.That(fifth.Key, Is.EqualTo("value5"));
            Assert.That(fifth.Value, Is.EqualTo(typeof(int)));
            Assert.That(sixth.Key, Is.EqualTo("value6"));
            Assert.That(sixth.Value, Is.EqualTo(typeof(int)));
            Assert.That(action.Aliases.First(), Is.EqualTo("value1"));
            Assert.That(action.Aliases.Skip(1).Take(1).First(), Is.EqualTo("value2"));
            Assert.That(action.Aliases.Skip(2).Take(1).First(), Is.EqualTo("value3"));
            Assert.That(action.Aliases.Skip(3).Take(1).First(), Is.EqualTo("value4"));
            Assert.That(action.Aliases.Skip(4).Take(1).First(), Is.EqualTo("value5"));
            Assert.That(action.Aliases.Skip(5).Take(1).First(), Is.EqualTo("value6"));
            action.Action(item, new object[] {1, 2, 3, 4, 5, 6});
            Assert.That(item.Sum, Is.EqualTo(21));
        }

        [Test]
        public void Act_Content()
        {
            var content = Substitute.For<IContent>();
            content.Get<int>(Arg.Any<string>()).Returns(1);
            var item = new TestAction();

            var map = new MapTask<TestAction>()
                .Act((c, x) =>
                {
                    x.Agregate = "value1 = " + c.Get<int>("value1");
                });

            Assert.That(map.Action, Is.Not.Null);
            map.Action(content, item);
            Assert.That(item.Agregate, Is.EqualTo("value1 = 1"));
        }

        [Test]
        public void Ignore_ok()
        {
            var map = new MapTask<TestItem>()
                .All();

            Assert.That(map.Maps.Count, Is.EqualTo(5));

            map.Ignore(x => x.ItemId);
            Assert.That(map.Maps.Count, Is.EqualTo(4));

            //again, no change
            map.Ignore(x => x.ItemId);
            Assert.That(map.Maps.Count, Is.EqualTo(4));
        }

        [Test]
        public void Fit_single()
        {
            var map = new MapTask<TestEntity>()
                .Fit(x => x.Item);

            var modelMap = map.ModelMaps.First();
            var entity = new TestEntity();
            var item = new TestItem();

            Assert.That(map.ModelMaps.Count, Is.EqualTo(1));
            Assert.That(modelMap.Alias, Is.EqualTo("item"));
            Assert.That(modelMap.GetKey, Is.EqualTo(ModelMap.DefaultGetKey));
            Assert.That(!modelMap.IsMany);
            modelMap.SetModel(entity, item);
            Assert.That(entity.Item, Is.EqualTo(item));
        }

        [Test]
        public void Fit_single_alias()
        {
            var map = new MapTask<TestEntity>()
                .Fit(x => x.Item, alias: "item-single");

            var modelMap = map.ModelMaps.First();
            var entity = new TestEntity();
            var item = new TestItem();

            Assert.That(map.ModelMaps.Count, Is.EqualTo(1));
            Assert.That(modelMap.Alias, Is.EqualTo("item-single"));
            Assert.That(modelMap.GetKey, Is.EqualTo(ModelMap.DefaultGetKey));
            Assert.That(!modelMap.IsMany);
            modelMap.SetModel(entity, item);
            Assert.That(entity.Item, Is.EqualTo(item));
        }

        [Test]
        public void Fit_single_with_keyFunc()
        {
            Func<TestItem, string> getKey = x => x.Key;

            var map = new MapTask<TestEntity>()
                .Fit(x => x.Item, getKey);

            var modelMap = map.ModelMaps.First();
            var entity = new TestEntity();
            var item = new TestItem();

            Assert.That(map.ModelMaps.Count, Is.EqualTo(1));
            Assert.That(modelMap.Alias, Is.EqualTo("item"));
            Assert.That(modelMap.GetKey, Is.Not.EqualTo(getKey));
            Assert.That(!modelMap.IsMany);
            modelMap.SetModel(entity, item);
            Assert.That(entity.Item, Is.EqualTo(item));
        }

        [Test]
        public void Fit_many()
        {
            var map = new MapTask<TestEntity>()
                .Fit(x => x.Items);

            var modelMap = map.ModelMaps.First();
            var entity = new TestEntity();
            var items = new[] { new TestItem() };

            Assert.That(map.ModelMaps.Count, Is.EqualTo(1));
            Assert.That(modelMap.Alias, Is.EqualTo("items"));
            Assert.That(modelMap.GetKey, Is.EqualTo(ModelMap.DefaultGetKey));
            Assert.That(modelMap.IsMany);
            modelMap.SetModel(entity, items);
            Assert.That(entity.Items, Is.EqualTo(items));
        }

        [Test]
        public void Fit_many_alias()
        {
            var map = new MapTask<TestEntity>()
                .Fit(x => x.Items, alias: "items-many");

            var modelMap = map.ModelMaps.First();
            var entity = new TestEntity();
            var items = new[] { new TestItem() };

            Assert.That(map.ModelMaps.Count, Is.EqualTo(1));
            Assert.That(modelMap.Alias, Is.EqualTo("items-many"));
            Assert.That(modelMap.GetKey, Is.EqualTo(ModelMap.DefaultGetKey));
            Assert.That(modelMap.IsMany);
            modelMap.SetModel(entity, items);
            Assert.That(entity.Items, Is.EqualTo(items));
        }

        [Test]
        public void Fit_many_with_keyFunc()
        {
            Func<TestItem, string> getKey = x => x.Key;

            var map = new MapTask<TestEntity>()
                .Fit(x => x.Items, getKey);

            var modelMap = map.ModelMaps.First();
            var entity = new TestEntity();
            var items = new[] { new TestItem() };

            Assert.That(map.ModelMaps.Count, Is.EqualTo(1));
            Assert.That(modelMap.Alias, Is.EqualTo("items"));
            Assert.That(modelMap.GetKey, Is.Not.EqualTo(getKey));
            Assert.That(modelMap.IsMany);
            modelMap.SetModel(entity, items);
            Assert.That(entity.Items, Is.EqualTo(items));
        }

        [Test]
        public void Fit_action()
        {
            var map = new MapTask<TestEntity>()
                .Fit<TestItem>((x, y) =>
                {
                    x.List = y.ToList();
                }, "items");

            var modelMap = map.ModelMaps.First();
            var entity = new TestEntity();
            var items = new List<TestItem> { new TestItem() };

            Assert.That(map.ModelMaps.Count, Is.EqualTo(1));
            Assert.That(modelMap.Alias, Is.EqualTo("items"));
            Assert.That(modelMap.GetKey, Is.EqualTo(ModelMap.DefaultGetKey));
            Assert.That(modelMap.IsMany);
            modelMap.SetModel(entity, items);
            Assert.That(entity.List, Is.EqualTo(items));
        }

        [Test]
        public void Fit_action_not_valid()
        {
            var map = new MapTask<TestEntity>()
                .Fit<TestItem>(null, "items")
                .Fit<TestItem>((x, y) =>
                {
                    x.List = y.ToList();
                }, null);

            Assert.That(map.ModelMaps.Count, Is.EqualTo(0));
        }

        [Test]
        public void Fit_action_with_keyFunc()
        {
            Func<TestItem, string> getKey = x => x.Key;

            var map = new MapTask<TestEntity>()
                .Fit((x, y) =>
                {
                    x.List = y.ToList();
                }, "items", getKey);

            var modelMap = map.ModelMaps.First();
            var entity = new TestEntity();
            var items = new List<TestItem> { new TestItem() };

            Assert.That(map.ModelMaps.Count, Is.EqualTo(1));
            Assert.That(modelMap.Alias, Is.EqualTo("items"));
            Assert.That(modelMap.GetKey, Is.Not.EqualTo(getKey));
            Assert.That(modelMap.IsMany);
            modelMap.SetModel(entity, items);
            Assert.That(entity.List, Is.EqualTo(items));
        }
    }
}
