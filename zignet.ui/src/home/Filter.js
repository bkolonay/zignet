import React, { Component } from 'react';

class Filter extends Component {

  render() {
    return (
      <div>
		<div className="form-check">
		  <input className="form-check-input" type="checkbox" value="" id="LoopNet"/>
		  <label className="form-check-label" htmlFor="LoopNet">LoopNet</label>
		</div>
		<div className="form-check">
		  <input className="form-check-input" type="checkbox" value="" id="LoopNet"/>
		  <label className="form-check-label" htmlFor="LoopNet">LM Mobile</label>
		</div>
		<div className="form-check">
		  <input className="form-check-input" type="checkbox" value="" id="LoopNet"/>
		  <label className="form-check-label" htmlFor="LoopNet">CityFeet</label>
		</div>
		<div className="form-check">
		  <input className="form-check-input" type="checkbox" value="" id="LoopNet"/>
		  <label className="form-check-label" htmlFor="LoopNet">Showcase</label>
		</div>
      </div>
    );
  }
}

export default Filter;
