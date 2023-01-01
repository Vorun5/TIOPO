import 'package:api_tests/answer.dart';
import 'package:api_tests/product.dart';
import 'package:dio/dio.dart';
import 'package:tuple/tuple.dart';

class ApiServices {
  static final _dio = Dio(BaseOptions(baseUrl: 'http://shop.qatl.ru/api'));

  static final apiError = Exception('api error');

  static Future<Tuple2<List<Product>, int>> getAllProducts() async {
    try {
      final response = await _dio.get('/products');
      // print(response.headers['content-type']);
      final products = (response.data as List)
          .map((dish) => Product.fromJson(dish as Map<String, dynamic>))
          .toList();

      return Tuple2(products, response.statusCode as int);
    } on DioError catch (e) {
      final response = e.response;
      if (response != null) {
        return Tuple2(
          [],
          response.statusCode as int,
        );
      }

      throw apiError;
    } catch (_) {
      throw apiError;
    }
  }

  static Future<Tuple2<Answer, int>> deleteProduct(int id) async {
    try {
      final response = await _dio.get('/deleteproduct?id=$id');
      // print(response.headers);

      return Tuple2(Answer.fromJson(response.data), response.statusCode as int);
    } on DioError catch (e) {
      final response = e.response;
      if (response != null) {
        return Tuple2(
          Answer.fromJson(response.data),
          response.statusCode as int,
        );
      }

      throw apiError;
    } catch (_) {
      throw apiError;
    }
  }

  static Future<Tuple2<Answer, int>> addProduct(Product product) async {
    try {
      final response = await _dio.post('/addproduct', data: product.toJson());

      return Tuple2(Answer.fromJson(response.data), response.statusCode as int);
    } on DioError catch (e) {
      final response = e.response;
      if (response != null) {
        return Tuple2(
          Answer.fromJson(response.data),
          response.statusCode as int,
        );
      }

      throw apiError;
    } catch (_) {
      throw apiError;
    }
  }

  static Future<Tuple2<Answer, int>> editProduct(Product product) async {
    try {
      final response = await _dio.post('/editproduct', data: product.toJson());

      return Tuple2(Answer.fromJson(response.data), response.statusCode as int);
    } on DioError catch (e) {
      final response = e.response;
      if (response != null) {
        return Tuple2(
          Answer.fromJson(response.data),
          response.statusCode as int,
        );
      }

      throw apiError;
    } catch (_) {
      throw apiError;
    }
  }
}
