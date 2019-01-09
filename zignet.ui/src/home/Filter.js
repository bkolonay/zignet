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
      <div className="row">
      	<div className="col">
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
				<div className="col">
					<div className="form-check">
					  <input 
					  	name="showDvm"
					  	type="checkbox"
					  	checked={this.props.filter.showDvm}
					  	onChange={this.handleCheckboxChange}
					  	className="form-check-input"
					  	id="showDvm"/>
					  <label className="form-check-label" htmlFor="showDvm">DVM</label>
					</div>
					<div className="form-check">
					  <input 
					  	name="showTsm"
					  	type="checkbox"
					  	checked={this.props.filter.showDvm}
					  	onChange={this.handleCheckboxChange}
					  	className="form-check-input"
					  	id="showTsm"/>
					  <label className="form-check-label" htmlFor="showTsm">TSM</label>
					</div>
					<div className="form-check">
					  <input 
					  	name="showTsr"
					  	type="checkbox"
					  	checked={this.props.filter.showTsr}
					  	onChange={this.handleCheckboxChange}
					  	className="form-check-input"
					  	id="showTsr"/>
					  <label className="form-check-label" htmlFor="showTsr">TSR</label>
					</div>
					<div className="form-check">
					  <input 
					  	name="showProd"
					  	type="checkbox"
					  	checked={this.props.filter.showProd}
					  	onChange={this.handleCheckboxChange}
					  	className="form-check-input"
					  	id="showProd"/>
					  <label className="form-check-label" htmlFor="showProd">Prod</label>
					</div>
					<div className="form-check">
					  <input 
					  	name="showProd"
					  	type="checkbox"
					  	checked={this.props.filter.showProd}
					  	onChange={this.handleCheckboxChange}
					  	className="form-check-input"
					  	id="showProd"/>
					  <label className="form-check-label" htmlFor="showProd">Dev</label>
					</div>
					<div className="form-check">
					  <input 
					  	name="showProd"
					  	type="checkbox"
					  	checked={this.props.filter.showProd}
					  	onChange={this.handleCheckboxChange}
					  	className="form-check-input"
					  	id="showProd"/>
					  <label className="form-check-label" htmlFor="showProd">Test</label>
					</div>					
				</div>
				<div className="col"/>
				<div className="col">
					<button onClick={this.handleClear} type="button" className="btn btn-link">Clear filters</button>
				</div>
      </div>
    );
  }
}

export default Filter;
