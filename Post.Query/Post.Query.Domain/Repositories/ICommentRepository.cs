﻿using Post.Query.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Query.Domain.Repositories
{
    public interface ICommentRepository
    {
        Task CreateAsync(CommentEntity comment);
        Task UpdateAsync(CommentEntity post);
        Task DeleteAsync(Guid commentId);
        Task<CommentEntity?> GetByIdAsync(Guid commentId);
    }
}
