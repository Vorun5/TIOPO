import 'package:freezed_annotation/freezed_annotation.dart';

part 'product.freezed.dart';
part 'product.g.dart';

@freezed
class Product with _$Product {
  factory Product({
    @Default(null) int? id,
    @Default(null) String? title,
    @Default(null) String? alias,
    @Default(null) String? content,
    @Default(null) double? price,
    @Default(null) int? status,
    @Default(null) String? keywords,
    @Default(null) String? description,
    @Default(null) int? hit,
    @Default(null) @JsonKey(name: 'category_id') int? categoryId,
    @Default(null) @JsonKey(name: 'old_price') double? oldPrice,
  }) = _Product;

  @override
  factory Product.fromJson(Map<String, dynamic> json) => _$_Product(
        id: int.parse(json['id']),
        title: json['title'] as String?,
        alias: json['alias'] as String?,
        content: json['content'] as String?,
        price: double.parse(json['price']),
        status: int.parse(json['status']),
        keywords: json['keywords'] as String?,
        description: json['description'] as String?,
        hit: int.parse(json['hit']),
        categoryId: int.parse(json['category_id']),
        oldPrice: double.parse(json['old_price']),
      );

  @override
  Map<String, dynamic> toJson() => <String, dynamic>{
        'id': id,
        'title': title,
        'alias': alias,
        'content': content,
        'price': price,
        'status': status,
        'keywords': keywords,
        'description': description,
        'hit': hit,
        'category_id': categoryId,
        'old_price': oldPrice,
      };
}

extension ProductEqual on Product {
  bool equal(Product product) {
    return copyWith(alias: null) == product.copyWith(alias: null);
  }
}
