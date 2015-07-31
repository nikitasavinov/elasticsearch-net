﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Nest
{
	public abstract class TypeDescriptorBase<TDescriptor, TInterface, T>
		: DescriptorBase<TDescriptor, TInterface>, IElasticType
		where TDescriptor : TypeDescriptorBase<TDescriptor, TInterface, T>, TInterface
		where TInterface : class, IElasticType
		where T : class
	{
		FieldName IElasticType.Name { get; set; }
		public TDescriptor Name(FieldName name) => Assign(a => a.Name = name);
		public TDescriptor Name(Expression<Func<TDescriptor, object>> objectPath) => Assign(a => a.Name = objectPath);

		TypeName IElasticType.Type { get; set; }

		string IElasticType.IndexName { get; set; }
		public TDescriptor IndexName(string indexName) => Assign(a => a.IndexName = indexName);

		bool IElasticType.Store { get; set; }
		public TDescriptor Store(bool store = true) => Assign(a => a.Store = store);

		bool IElasticType.DocValues { get; set; }
		public TDescriptor DocValues(bool docValues = true) => Assign(a => a.DocValues = docValues);

		IDictionary<FieldName, IElasticType> IElasticType.Fields { get; set; }
		public TDescriptor Fields(Func<PropertiesDescriptor<T>, PropertiesDescriptor<T>> selector) => Assign(a =>
		{
			selector.ThrowIfNull(nameof(selector));
			var properties = selector(new PropertiesDescriptor<T>());
			foreach (var property in properties.Properties)
			{
				var value = property.Value as IElasticType;
				if (value == null)
					continue;
				a.Fields[property.Key] = value;
			}
		});

		SimilarityOption IElasticType.Similarity { get; set; }
		public TDescriptor Similarity(SimilarityOption similarity) => Assign(a => a.Similarity = similarity);

		IEnumerable<FieldName> IElasticType.CopyTo { get; set; }
		public TDescriptor CopyTo(IEnumerable<FieldName> copyTo) => Assign(a => a.CopyTo = copyTo);

		object IElasticType.Fielddata { get; set; }
		public TDescriptor Fielddata(object fieldData) => Assign(a => a.Fielddata = fieldData);
	}
}