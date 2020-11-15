using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentBlog.Models;

namespace FluentBlog.DataRepositories
{
    public interface IMetaRepository
    {
        // 增
        Meta Insert(Meta meta);
        // 删
        Meta Delete(int id);
        // 改
        Meta Update(Meta updateMeta);
    }
}
