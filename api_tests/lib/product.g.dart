// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'product.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

_$_Product _$$_ProductFromJson(Map<String, dynamic> json) => _$_Product(
      id: json['id'] as int? ?? null,
      title: json['title'] as String? ?? null,
      alias: json['alias'] as String? ?? null,
      content: json['content'] as String? ?? null,
      price: (json['price'] as num?)?.toDouble() ?? null,
      status: json['status'] as int? ?? null,
      keywords: json['keywords'] as String? ?? null,
      description: json['description'] as String? ?? null,
      hit: json['hit'] as int? ?? null,
      categoryId: json['category_id'] as int? ?? null,
      oldPrice: (json['old_price'] as num?)?.toDouble() ?? null,
    );

Map<String, dynamic> _$$_ProductToJson(_$_Product instance) =>
    <String, dynamic>{
      'id': instance.id,
      'title': instance.title,
      'alias': instance.alias,
      'content': instance.content,
      'price': instance.price,
      'status': instance.status,
      'keywords': instance.keywords,
      'description': instance.description,
      'hit': instance.hit,
      'category_id': instance.categoryId,
      'old_price': instance.oldPrice,
    };
