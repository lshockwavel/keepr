using Keepr.Models;
using Keepr.Repositories;

namespace Keepr.Services;

public class KeepService
{
    private readonly KeepsRepository _repo;

    public KeepService(KeepsRepository repo)
    {
        _repo = repo;
    }

    internal Keep CreateKeep(Keep newKeep)
    {
        Keep createdKeep = _repo.CreateKeep(newKeep);
        return createdKeep;
    }

    internal string DeleteKeep(int keepId, string id)
    {
        Keep keep = GetKeepById(keepId);
        if (keep.CreatorId != id)
        {
            throw new Exception("You are not the creator of this keep. Odin is watching.");
        }

        _repo.DeleteKeep(keepId);
        return "Keep Deleted";
    }

    internal List<Keep> GetAllKeeps()
    {
        List<Keep> keeps = _repo.GetAllKeeps();
        return keeps;
    }

    internal List<Keep> GetAllKeeps(string id)
    {
        List<Keep> keeps = GetAllKeeps();

        //If there is a user id, filter out vault keeps not created by that user
        // keeps = keeps.FindAll(keep => keep.CreatorId == id); //REVIEW Maybe add is_private field to keep model?
        return keeps;
    }

    internal Keep GetKeepById(int keepId)
    {
        Keep keep = _repo.GetKeepById(keepId);
        return keep;
    }

    internal Keep IncrementKeepViews(int keepId)
    {
        Keep keep = GetKeepById(keepId);
        if (keep == null)
        {
            throw new Exception("Invalid Keep Id");
        }

        keep.Views++;
        _repo.incrementKeepViews(keep);
        return keep;
    }

    internal Keep UpdateKeep(Keep updatedKeep, string id)
    {
        Keep originalKeep = GetKeepById(updatedKeep.Id);
        if (originalKeep.CreatorId != id)
        {
            throw new Exception("You are not the creator of this keep. Odin is watching.");
        }

        originalKeep.Name = updatedKeep.Name ?? originalKeep.Name;
        originalKeep.Description = updatedKeep.Description ?? originalKeep.Description;
        originalKeep.Img = updatedKeep.Img ?? originalKeep.Img;  //REVIEW Should be able to edit? I think it should be fine?

        _repo.UpdateKeep(originalKeep);
        return originalKeep;
    }
}