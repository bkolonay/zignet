import React, { Component } from 'react';
import { getDefaultFilter } from '../common/routing/SearchFilter.js'

class Filter extends Component {
	constructor(props) {
		super(props)

		this.handleCheckboxChange = this.handleCheckboxChange.bind(this);
		this.handleClear = this.handleClear.bind(this);
	}

	handleCheckboxChange(event) {
		this.props.filter[event.target.name] = event.target.checked;
		this.props.onFilterChange(this.props.filter);
	}

	handleClear(event) {
		event.preventDefault();
		this.props.onFilterChange(getDefaultFilter());
	}

  render() {
    return (
      <div>
      	<button onClick={this.handleClear} type="button" className="btn btn-link">Clear</button>
				<div className="form-check">
				  <input 
				  	name="showLoopNet"
				  	type="checkbox"
				  	checked={this.props.filter.showLoopNet}
				  	onChange={this.handleCheckboxChange}
				  	className="form-check-input"
				  	id="showLoopNet"/>
				  <label className="form-check-label" htmlFor="showLoopNet">LoopNet</label>
				</div>
				<div className="form-check">
				  <input 
				  	name="showLmMobile"
				  	type="checkbox"
				  	checked={this.props.filter.showLmMobile}
				  	onChange={this.handleCheckboxChange}
				  	className="form-check-input"
				  	id="showLmMobile"/>
				  <label className="form-check-label" htmlFor="showLmMobile">LM Mobile</label>
				</div>
				<div className="form-check">
				  <input 
				  	name="showCityFeet"
				  	type="checkbox"
				  	checked={this.props.filter.showCityFeet}
				  	onChange={this.handleCheckboxChange}
				  	className="form-check-input"
				  	id="showCityFeet"/>
				  <label className="form-check-label" htmlFor="showCityFeet">CityFeet</label>
				</div>
				<div className="form-check">
				  <input 
				  	name="showShowcase"
				  	type="checkbox"
				  	checked={this.props.filter.showShowcase}
				  	onChange={this.handleCheckboxChange}
				  	className="form-check-input"
				  	id="showShowcase"/>
				  <label className="form-check-label" htmlFor="showShowcase">Showcase</label>
				</div>				
      </div>
    );
  }
}

export default Filter;
