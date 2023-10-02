﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameDevUtils.StateManagement
{
    public class Puppeteer
    {
        #region Class Members
        private Animator animator;
        #endregion

        #region Properties
        public List<string> Parameters => (from state in animator.parameters select state.name).ToList();
        public IDictionary<string, bool> BoolParams;
        public IDictionary<string, float> FloatParams;
        public IDictionary<string, int> IntParams;
        #endregion

        public Puppeteer(Animator animator) {
            this.animator = animator;
            this.BoolParams = new Dictionary<string, bool>();
            this.FloatParams = new Dictionary<string, float>();
            this.IntParams = new Dictionary<string, int>();
        }

        /// <summary>
        /// Set a boolean parameter's value.
        /// </summary>
        /// <param name="param">The parameter's name</param>
        /// <param name="flag">The new parameter value</param>
        public void Manipulate(string param, bool flag) {
            animator.SetBool(param, flag);
            BoolParams[param] = flag;
        }

        /// <summary>
        /// Set a float parameter's value.
        /// </summary>
        /// <param name="param">The parameter's name</param>
        /// <param name="value">The new parameter value</param>
        public void Manipulate(string param, float value) {
            animator.SetFloat(param, value);
            FloatParams[param] = value;
        }

        /// <summary>
        /// Set an integer parameter's value.
        /// </summary>
        /// <param name="param">The parameter's name</param>
        /// <param name="value">The new parameter value</param>
        public void Manipulate(string param, int value) {
            animator.SetInteger(param, value);
            IntParams[param] = value;
        }

        /// <summary>
        /// Activate a trigger parameter.
        /// </summary>
        /// <param name="param">The parameter's name</param>
        public void Manipulate(string param) {
            animator.SetTrigger(param);
        }
    }
}