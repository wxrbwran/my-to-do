﻿using MyToDo.Shared;
using MyToDo.Shared.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDO.Service
{
  public interface IBaseService<TEntity> where TEntity : class
  {
    Task<ApiResponse<TEntity>> AddAsync(TEntity entity);
    Task<ApiResponse> DeleteAsync(int id);
    Task<ApiResponse<TEntity>> UpdateAsync(TEntity entity);
    Task<ApiResponse<TEntity>> GetAsync(int id);

    Task<ApiResponse<PagedList<TEntity>>> GetAllAsync(QueryParameter parameter);
  }
}
