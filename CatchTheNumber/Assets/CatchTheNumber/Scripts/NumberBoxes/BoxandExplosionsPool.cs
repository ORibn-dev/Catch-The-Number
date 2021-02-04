using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BoxandExplosionsPool : MonoBehaviour
{
    #region DeclaredFields

    [SerializeField] private NumberBox number_box;
    [SerializeField] private ParticleSystem destruction_particle, 
        destruction_particle_catch, destruction_particle_ground;

    public List<NumberBox> pooled_boxes;
    public List<ExplosionPools> explosion_pools;
    private Queue<ParticleSystem> explosionstodisable = new Queue<ParticleSystem>();

    private NumberBox pooledbox;
    private ParticleSystem pooledexp, availableexp;

    #endregion

    #region Initialization

    public static BoxandExplosionsPool Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        PooledBoxesAndExplosions();
    }

    #endregion

    #region CreateObjectPools

    public void PooledBoxesAndExplosions()
    {
        for (int i = 0; i < 15; i++)
        {
            InstPooledStuff<NumberBox>(pooledbox, number_box, pooled_boxes);
            InstPooledStuff<ParticleSystem>(pooledexp, destruction_particle, explosion_pools[0].pooled_explosions);
            InstPooledStuff<ParticleSystem>(pooledexp, destruction_particle_catch, explosion_pools[1].pooled_explosions);
            InstPooledStuff<ParticleSystem>(pooledexp, destruction_particle_ground, explosion_pools[2].pooled_explosions);
        }
    }

    private void InstPooledStuff<T>(T obj, T objtoinst, List<T> pooledobjs) where T : Component
    {
        obj = (T)Instantiate(objtoinst);
        obj.gameObject.SetActive(false);
        pooledobjs.Add(obj);
    }

    #endregion

    #region GetObjectFromAPool

    public T GetStuff<T>(List <T> pooled_stuff) where T : Component
    {
        for (int x = 0; x < pooled_stuff.Count; x++)
        {
            if (!pooled_stuff[x].gameObject.activeInHierarchy)
            {
                return pooled_stuff[x];
            }
        }
        return default(T);
    }

    #endregion

    #region SetExplosion

    public void InitiateExplosion(int explosion_indx, Vector3 pos, Quaternion rot)
    {
        StartCoroutine(InititateExplosionProccess(explosion_indx, pos, rot));
    }
    private IEnumerator InititateExplosionProccess(int explosion_indx, Vector3 pos, Quaternion rot)
    {
        availableexp = GetStuff<ParticleSystem>(explosion_pools[explosion_indx].pooled_explosions);
        availableexp.transform.position = pos;
        availableexp.transform.rotation = rot;
        availableexp.gameObject.SetActive(true);
        availableexp.Play();
        explosionstodisable.Enqueue(availableexp);
        yield return new WaitForSeconds(2f);
        explosionstodisable.Dequeue().gameObject.SetActive(false);
    }

    #endregion
}
[Serializable]
public struct ExplosionPools
{
    public List<ParticleSystem> pooled_explosions;
}
